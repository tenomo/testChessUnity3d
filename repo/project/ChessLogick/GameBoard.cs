using System;
using System.Collections.Generic;
 

namespace Chess
{
    class GameBoard : Singleton<GameBoard>
    {
        /// <summary>
        /// Список фигур на доске.
        /// </summary>
        public  List<Figure> Figures { get; set; }
        /// <summary>
        /// Список поверженых фигур.
        /// </summary>
        public   List<Figure> DeathFigures { get; set; }

        //Magic name?????? 
        //Magic number indicates the width, height.
        /// <summary>
        /// 
        /// </summary>
        public  byte width = 8, height = 8, zero = 1;      

        /// <summary>
        /// Входит ли заданая точка в пределы доски.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>        
        public   bool isOnTheChessboard(PointV2 point)
        {
            if (point.x <= width && point.x >= zero && point.y <= height && point.y >= zero)
                return true;
            else return false;
        }   


        private GameBoard () : base ()
        {
            DeathFigures = new List<Figure>();
            Figures = new List<Figure>();
            
        }
        /// <summary>
        /// Возвращат найденую фигуру. Если фигура не найдена, null. 
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Figure GetFigureOfPosition (PointV2 pos)
        {
            if (Figures.Exists(x => x.position.Equals(pos)))
                return Figures.Find(x => x.position.Equals(pos));
            else
                return null;
        }

        public void AlignmentGamePieces(/*IBuildeGameBoard buildeGameBoard*/)
        {
            GameFiguresBuilder builder = new GameFiguresBuilder();
            this.Figures = builder.Build();
        }

    }
}
