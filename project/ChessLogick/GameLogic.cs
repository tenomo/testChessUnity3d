using System;
using System.Collections.Generic;
 using System.ComponentModel;
namespace Chess
{
    class GameLogic : Singleton<GameLogic>
    {


        [DefaultValue(new Player[] { new Player(CellStatus.Player1), new Player(CellStatus.Player2) })]
        public Player[] Players { get; set; }
        public bool isCanWalk { get; private set; }
        
        private int _playerIndex = 0;
        private int playerIndex
        {
            get { return _playerIndex; }
            set
            {
                if (value > 1)
                    _playerIndex = 0;
                else if (value < 0)
                    _playerIndex = 1;
                else
                    _playerIndex = value;
            }
        }
        private GameLogic()
            : base()
        {
            playerIndex = 0;
        }
        // GameLogic 
        
        Player currentPlayer;
        public Player GetPlayerWhoGoes()
        {
           
            
                if (!Players[playerIndex - 1].isWalking)
                {
                   // this.CheckMate();
                    this.CheckShah();
                    if (isCanWalk)
                    {
                        currentPlayer = Players[playerIndex];
                        playerIndex++;
                        return currentPlayer;
                    }
                    return null;
                }
                else
                {
                    throw new MissingMethodException("Previous player still walks.");
                }
        }

        private void CheckShah()
        {
           PointV2 kingPos  = currentPlayer.Figures.Find(x=>x.name == "king").position;
           foreach (var item in Players[playerIndex -1].Figures)
           {
               if (item.CellsForAttacks.Exists(x => x.Equals(kingPos))) /*Событие шах*/;

           }
        }
        
        

    }
    class Player 
    {
          CellStatus Side;
          public bool isWalking { get; set; }
          public CellStatus side
          {

              get { return this.Side; }
              set
              {
                  if (value != CellStatus.AbroadChessboard && value != CellStatus.VoidCell) ;
                  else
                      throw new MissingMethodException("Playr.side не может равняться AbroadChessboard или VoidCell");
              }
          }
        public List<Figure> Figures
        {
            get
            { 
                List<Figure> figures = new List<Figure>();
                foreach (var item in GameBoard.Instance.Figures)
                {
                    if (item.side == this.side)
                        figures.Add(item);
                }
                return figures;
            }
            private set { }
        }


 

        public Figure GeFigure (PointV2 pos)
        {
           return Figures.Find(x => x.Equals(pos));
        }

        public Player (CellStatus side)
        {
            this.side = side;
        }
    }
}
