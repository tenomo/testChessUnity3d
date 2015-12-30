using System;
 

namespace Chess
{
    /// <summary>
    /// Ходит на одно поле по вертикали вперёд. Из исходного положения может сделать один ход на два поля вперёд. Бьёт на одно поле по диагонали вперёд.
    public class Pawn : Figure
    {


        // test event

        public override event Figure.DontActionDelegate DontActioEvent;

        public Pawn(CellStatus side, PointV2 position)
            : base(side, position)
        {
            isFirstStep = true;
        }
        private bool isFirstStep { get; set; }
        /// <summary>
        /// По правилам шахмат: Первый ход, пешк может ходить на 2 клетки. 
        /// </summary>
        private byte CountStep
        {
            get
            {
                if (isFirstStep)
                    return 2;
                else
                    return 1;
            }

        }

        public override void Walk(PointV2 direction)
        {
            FormulatedAvailableActions();

            if (CellsForWalks.Count > 0)
                if (base.GetIsAvailableForActions(direction))
                {
                    this.position = direction;
                    if (isFirstStep)
                        isFirstStep = false;
                }
                else DontActioEvent("Не возможно выполнить действие относительно клетки по данным координатам");
            // throw new System.Exception(AbroadChessboardException);
            //==================================================================
            else DontActioEvent("Фигура не имеет ходов");// СОБЫТИЕ СООБЩАЮЩЕ ЧТО ДАННАЯ ФИГУРА НЕ ИМЕЕТ ДОСТУПНЫХ ХОДОВ 
            //===================================================================
        }

        //private string AbroadChessboardException = "direction не входит в список" +
         //           "координат клеток, доступных для действий. Рекомендуеться использовать координаты из списков CellsForAttacks ии CellsForWalks";



        /// <summary>
        /// Формулирует список координат клеток, с которыми возможно взаимодействие: атака, ход.
        /// </summary>
        /// <returns></returns>
        protected override void FormulatedAvailableActions()
        {
            this.CellsForAttacks.Clear();
            this.CellsForWalks.Clear();
            PointV2 tmpCoordinate;
            for (int i = 1; i <= CountStep; i++)
            {
                tmpCoordinate = new PointV2(this.position.x, this.position.y + i);
                if (GetStatusCell(tmpCoordinate) == CellStatus.VoidCell)
                    CellsForWalks.Add(tmpCoordinate);
                else break;
            }

            tmpCoordinate = position + 1;
            if (isCellForAttack(tmpCoordinate))
            {  
                CellsForAttacks.Add(tmpCoordinate);
                CellsForWalks.Add(tmpCoordinate);
            }

            tmpCoordinate = new PointV2(position.x - 1, position.y + 1);
            if (isCellForAttack(tmpCoordinate))
            {  
                CellsForAttacks.Add(tmpCoordinate);
                CellsForWalks.Add(tmpCoordinate);
            }
             
        }
        protected override void Initialize()
        {
            base.Initialize();
            name = "Pawn";
            id = 0;
            FocusFigureEvent += Pawn_FocusFigureEvent;
        }

        void Pawn_FocusFigureEvent(IFigure focusFigure)
        {
            this.FormulatedAvailableActions();
        }
    }
}