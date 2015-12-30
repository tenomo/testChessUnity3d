using System.Collections.Generic;

namespace Chess
{
    class GameFiguresBuilder  // Наследывать от интерфейса IBuilder
    {
        private List<Figure> Figures;
        public GameFiguresBuilder()
        {
            Figures = new List<Figure>();
        }
       
        public List<Figure> Build ()
         {
             CellStatus side = CellStatus.Player1;
             int y = 1;

             for (int i = 0; i < 2; i++)
             {
                 Figures.Add(new Сastle(side, new PointV2(8, y)));
                 Figures.Add(new Сastle(side, new PointV2(1, y)));

                 Figures.Add(new Bishop(side, new PointV2(7, y)));
                 Figures.Add(new Bishop(side, new PointV2(2, y)));

                 Figures.Add(new Horse(side, new PointV2(6, y)));
                 Figures.Add(new Horse(side, new PointV2(3, y)));



                 for (int iPawn = 1; iPawn <= 8; iPawn++)
                 {
                     Figures.Add(new Pawn(side, new PointV2(iPawn, y)));
                 }
                 side = CellStatus.Player2;
                 y = 8;
             }            
            
            Figures.Add(new King(side, new PointV2(5, 1)));
            Figures.Add(new Queen(side, new PointV2(4, 1)));

            Figures.Add(new King(side, new PointV2(4, 8)));
            Figures.Add(new Queen(side, new PointV2(5, 8)));

            return Figures;
         }
        
    }

     
}
