 

namespace Chess
{
    class Queen : Figure
    {

        public Queen(CellStatus side, PointV2 position) : base(side, position) { }

        protected override void FormulatedAvailableActions()
        { 

            this.CellsForWalks.AddRange(ManagerAlgorithmsMotion.GetHorisontal(this, false));
            this.CellsForWalks.AddRange(ManagerAlgorithmsMotion.GetVertical(this, false));
            this.CellsForWalks.AddRange(ManagerAlgorithmsMotion.GetDiagonaleA(this, false));
            this.CellsForWalks.AddRange(ManagerAlgorithmsMotion.GetDiagonaleB(this, false));
        }
        public override event DontActionDelegate DontActioEvent;
        protected override void Initialize()
        {
            base.Initialize();
            base.Initialize();
            id = 5;
            name = "Queen";
            FocusFigureEvent += Queen_FocusFigureEvent;
        }
        void Queen_FocusFigureEvent(IFigure focusFigure)
        {
            FormulatedAvailableActions();
        }
         
        }
}
