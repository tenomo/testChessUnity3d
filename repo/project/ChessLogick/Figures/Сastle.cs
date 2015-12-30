using System;
using System.Collections.Generic;
 

namespace Chess
{
    class Сastle : Figure
    {

        public  Сastle (CellStatus side, PointV2 position) : base(side, position)
        { 
        }

        
        // Формирет списки доступных клеток Сastle
        protected override void FormulatedAvailableActions()
        {
            this.CellsForWalks.AddRange(ManagerAlgorithmsMotion.GetHorisontal(this, false));
            this.CellsForWalks.AddRange(ManagerAlgorithmsMotion.GetVertical(this, false)); 

        }
        //
        /// <summary>
        /// Обработчик события CastlingEvent
        /// </summary>
        /// <param name="k"></param>
        public void Сastle_CastlingEvent (King k)
        {
            PointV2 k_position = k.position;
            k.position = this.position;
            this.position = k_position;
        }

        public override event DontActionDelegate DontActioEvent;
        protected override void Initialize()
        {
            base.Initialize();
            id = 3;
            name = "Сastle";
            FocusFigureEvent += Сastle_FocusFigureEvent;
        }

        void Сastle_FocusFigureEvent(IFigure focusFigure)
        {
            FormulatedAvailableActions();
        }
     }
}
