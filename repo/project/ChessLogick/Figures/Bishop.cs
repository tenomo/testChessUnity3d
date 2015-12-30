namespace Chess
{
    /// <summary>
    /// Ходит на любое число полей по диагоналям. В начале игры у игрока есть два слона — белопольный и чёрнопольный. 
    /// В силу геометрии шахматной доски, слоны перемещаются только по диагоналям своего цвета. 
    /// </summary>
    class Bishop : Figure
    {

        public Bishop(CellStatus side, PointV2 position) : base(side, position) { }

       
        protected override void FormulatedAvailableActions()
        {
             
            this.CellsForWalks.AddRange(ManagerAlgorithmsMotion.GetDiagonaleA(this, false));
            this.CellsForWalks.AddRange(ManagerAlgorithmsMotion.GetDiagonaleB(this, false));
        }

        public override event DontActionDelegate DontActioEvent;
        protected override void Initialize()
        {
            base.Initialize();
            id = 4;
            name = "Bishop";
            FocusFigureEvent += Bishop_FocusFigureEvent;
        }

        void Bishop_FocusFigureEvent(IFigure focusFigure)
        {
            FormulatedAvailableActions();
        }
    }



  
}