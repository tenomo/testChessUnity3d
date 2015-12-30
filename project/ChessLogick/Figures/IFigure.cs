using System;
using System.Collections.Generic;

namespace Chess
{
    public interface IFigure
    {

        CellStatus side { get; set; }
        PointV2 position { get; set; }
        byte id { get; set; }
        string name { get; set; }
        List<PointV2> CellsForWalks { get; set; }
        List<PointV2> CellsForAttacks { get; set; }
    }

    public interface IMotionInterfaceFigure
    {

        PointV2 position { get; set; }
        bool GetIsAvailableForActions(PointV2 direction);
    }
}