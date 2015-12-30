using System;
 

namespace Chess
{
    /// <summary>
    /// Ходит русской буквой «Г» (или латинской буквой «L») — сначала на два поля по вертикали или горизонтали, 
    /// потом ещё на одно поле по горизонтали или вертикали перпендикулярно первоначальному направлению. 
    /// Единственная фигура в современных шахматах, которая ходит не по прямой линии и является «прыгающей» — может 
    /// «перепрыгивать» через свои и неприятельские фигуры
    /// </summary>
    class Horse : Figure
    {

        public Horse(CellStatus side, PointV2 position)
            : base(side, position)
        {

        }

        /// <summary>
        /// Формирует список доступных клеток для "Коня".
        /// </summary>
        protected override void FormulatedAvailableActions()
        {
            this.CellsForAttacks.Clear();
            this.CellsForWalks.Clear();
            PointV2[] dirList = { PointV2.down, PointV2.left, PointV2.up, PointV2.right };
            PointV2 tmpPoint = new PointV2(this.position.x + 2, this.position.y + 2);
            foreach (var itemDirList in dirList)
                for (int i = 1; i < 5; i++)
                {
                    tmpPoint += itemDirList;
                    if (i % 2 == 1 && GameBoard.Instance.isOnTheChessboard(tmpPoint))
                    {
                        if (GetStatusCell(tmpPoint) != side)
                            CellsForWalks.Add(tmpPoint);
                        if (base.isCellForAttack(tmpPoint))
                            CellsForAttacks.Add(tmpPoint);
                    }
                }
        }
        public override event DontActionDelegate DontActioEvent;
        protected override void Initialize()
        {
            base.Initialize();
            id = 1;
            name = "Knight";
            FocusFigureEvent += Horse_FocusFigureEvent;
        }

        void Horse_FocusFigureEvent(IFigure focusFigure)
        {
            FormulatedAvailableActions();
        }
    }
}
