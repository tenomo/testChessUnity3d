using System;
using System.Collections.Generic;
 

namespace Chess
{
    class King : Figure
    {
        /// <summary>
        /// Делегирует обработчик события CastlinEvent. Передает ссылку на экземпляр класса King.
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public delegate void CastlingEventHandler(King k);
        public event CastlingEventHandler CastlingEvent;

        public void Castling(Сastle c)
        {
            this.CastlingEvent += c.Сastle_CastlingEvent;
            if (this.CastlingEvent != null)
                CastlingEvent(this);
        }


       
      protected override void FormulatedAvailableActions()
      {
          ClerrAvaileCells();
       

    this.CellsForWalks.AddRange(       ManagerAlgorithmsMotion.GetHorisontal(this, true));
    this.CellsForWalks.AddRange(ManagerAlgorithmsMotion.GetVertical(this, true));
    this.CellsForWalks.AddRange(ManagerAlgorithmsMotion.GetDiagonaleA(this, true));
    this.CellsForWalks.AddRange(ManagerAlgorithmsMotion.GetDiagonaleB(this, true));

          // Если найлена фигура опонента, своим положением доступна для действий данной фигуры 
          // Вносим ее координаті в список доступныых для атаки клеток.
          foreach (var item in this.CellsForWalks)
          {
             
              if (this.isCellForAttack(item)) 
                  this.CellsForAttacks.Add(item);
          }
           
      }
        public override event DontActionDelegate DontActioEvent;
        protected override void Initialize()
        {
            base.Initialize();
            id = 6;
            name = "King";
            FocusFigureEvent += King_FocusFigureEvent;
        }

        void King_FocusFigureEvent(IFigure focusFigure)
        {
            FormulatedAvailableActions();
        }


        public King(CellStatus side, PointV2 position) : base(side, position) { }
    }
}
