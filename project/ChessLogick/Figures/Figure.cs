using System;
using System.Collections.Generic;
 


namespace Chess
{
  
    /// <summary>
    /// Состояние клетки соотвецтвенно заданых, которое отображает: принадлежит клетка игроку(сторона), 
    /// пустая клетка, или же координата не входит в ганицу шахматной доски.
    /// </summary>
    public enum CellStatus { Player1, Player2, VoidCell, AbroadChessboard = 0 };

    /// <summary>
    /// Общее, базовое описание Шахматной фигуры.
    /// </summary>
    public abstract class Figure : IFigure , IMotionInterfaceFigure
    {
        public Figure(CellStatus side, PointV2 position)
        {
            this.position = position;
            this.side = side;
            this.Initialize();
        }       

        public  delegate void DontActionDelegate(string str);
        public virtual event DontActionDelegate DontActioEvent;

        /// <summary>
        ///  Инициализация основнх параметров фигуры.
        /// </summary>
        protected virtual void Initialize()
        {
            CellsForAttacks = new List<PointV2>();
            CellsForWalks = new List<PointV2>();
        }
        // Сторона фигурі: Игрок1, Игрок2, пустая клетка, кордината вышла за рамки. 
        // При возвращенни AbroadChessboard необходимо вызывать исключение.
        public CellStatus side { get;  set; }
       // public int AmoutCellsAllForAWalks { get { return CellsForWalks.Count; } }
        public int AmoutCellsForWalks { get { return CellsForWalks.Count - AmoutCellsForAttacks; } }
        public int AmoutCellsForAttacks { get { return CellsForWalks.Count; } }

        private PointV2 _position;
        /// <summary>
        /// Позиция фигуры.
        /// </summary>
        public PointV2 position
        {
            get { return _position; }
              set
            {
                if (GameBoard.Instance.isOnTheChessboard(value))
                    _position = value;
                else
                    DontActioEvent("Координата фигуры вне шахматной доски");
            }
        }
        /// <summary>
        ///  Список координат доступных для хода (Включает в себя клетки как для хода так и для атаки).
        /// </summary>
        public List<PointV2> CellsForWalks { get; set; }   
        /// <summary>
        /// Список координат достапных для атаки.
        /// </summary>
        public List<PointV2> CellsForAttacks { get;   set; } // Доступные клетки исключительно для атаки (если таковы имеються)

      /// <summary>
      /// Ход на заданую позицию.
      /// </summary>
      /// <param name="direction"></param>
        public virtual void Walk(PointV2 direction)
        {
            // Формирует данные о доступных клетках для данной фигуры: ходы, атаки;
            // для дальнейших проверок.
            FormulatedAvailableActions(); // == Нет необходимости == 
            if (CellsForWalks.Count > 0)
                if (GetIsAvailableForActions(direction))
                {

                    //UnityEngine.Debug.LogError("WALk");
                    if (isCellForAttack(direction))
                    {
                        //UnityEngine.Debug.LogError("Death");
                        GameBoard.Instance.GetFigureOfPosition(direction).Death();
                    }
                    
                    this.position = direction;
                }
        
                else DontActioEvent("Не возможно выполнить действие относительно клетки по данным координатам");
            // throw new System.Exception(AbroadChessboardException);
            //==================================================================
            else DontActioEvent("Фигура не имеет ходов");// СОБЫТИЕ СООБЩАЮЩЕ ЧТО ДАННАЯ ФИГУРА НЕ ИМЕЕТ ДОСТУПНЫХ ХОДОВ 
            //===================================================================
        } 

        /// <summary>
        ///  Формирует список координат клеток, с которыми возможно взаимодействие: атака, ход.   
        /// </summary>
        protected abstract void FormulatedAvailableActions();

        /// <summary>
        ///  Очищает данные о доступных клетках.
        /// </summary>
        protected void ClerrAvaileCells ()
        {
            CellsForAttacks.Clear();
            CellsForWalks.Clear();
        } 
   


        /// <summary>
        /// Возвращает статус клетки: Сторона игрока, пустая клетка, координата вышла за границу доски.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        protected CellStatus GetStatusCell(PointV2 direction)
        {
            //  Если клетка входит в границы игрового поля.
            if (GameBoard.Instance.isOnTheChessboard(direction))
            {
                // Если на игровой доске есть фигура с задаными координатами, возвращаем ее статус. 
                if (GameBoard.Instance.Figures.Exists(x => x.position.Equals(direction)))
                    return GameBoard.Instance.Figures.Find(x => x.position.Equals(direction)).side;
                 // Иначе клетка пустая.
                else return CellStatus.VoidCell;
            }
            else return CellStatus.AbroadChessboard;
        }

        /// <summary>
        /// Возвращает: доступна ли клетка по заданым координатам для каких либо дейсвтий данной фигуре.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public bool GetIsAvailableForActions(PointV2 direction)
        {
            if (GameBoard.Instance.isOnTheChessboard(direction) && GetStatusCell(direction) != this.side)
            { 
                return true;
            }
            else
            { 
                return false;
            }
        }

        // ============ ФОКУС===========================================================

        public  delegate void FocusFigure (IFigure focusFigure) ;//- возможно интерфейс клетки:  <===========================
        //                              список клеток(атака, ход), позция, сторона, ид, имя <===
         public event FocusFigure FocusFigureEvent;
         
        /// <summary>
        /// Если клетка занята фигурой опонента, возвращает true
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public bool isCellForAttack(PointV2 dir)
        {
            CellStatus CS = GetStatusCell(dir);
            // Если клетка по данным координатам не пуста, входит в границы доски,
            // сторона фигуры не являеться дружественой - можно ее атаковать. 
            if (this.GetIsAvailableForActions(dir) && CS != CellStatus.VoidCell)           
                    return true;               
                else 
                return false;       
        }

       /// <summary>
       ///  Оповещает что данная фигура в фокусе.
       /// </summary> 
        public virtual void Focus()
        {
            if (FocusFigureEvent != null)
                FocusFigureEvent(this);
        }


        public delegate void DeathEventHandler(Figure obj);
        public event DeathEventHandler DeathEvent;

        public void Death()
        {
           // GameBoard.Instance.DeathFigures.Add(this);
           // GameBoard.Instance.Figures.Remove(this);
            //                   ПРОДУМАТЬ МЕХАНИКУ
            // Оповестить всех заинтересованых о своей смерти
            // Наблюдатель?  есть ли смысл следить за каждой фигурой?
            // Нет, метод сообщает о своей смерти по средством прямого обращения.

            if (this.DeathEvent != null)
                DeathEvent(this);
        }

        #region Члены IFocusFigure
        public byte id{get;  set;}
        public string name { get;   set; }

        #endregion

        // Делает копию текушего обьекта
        #region prototype region
        public Figure Clone ()
        {  
            return (Figure)this.MemberwiseClone();
        }
        #endregion prototype region
    }
    

  
     
}