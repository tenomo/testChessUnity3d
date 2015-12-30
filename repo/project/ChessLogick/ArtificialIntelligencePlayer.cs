using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using  
namespace Chess
{
    
    class ArtificialIntelligencePlayer : Singleton<ArtificialIntelligencePlayer>
    {
        // конектимся к базе
        // Посылаем текущую ситуацию на сервер
        // Сервер ищет лучшее решение
        // Отправляе сда
        // Возвращаем ход игроку.

        public ArtificialIntelligencePlayer () : base ()
    {

    }
    }
   
    
    [Serializable()]
    class MementoStep   
    {
        public PointV2 position { get; set; }
        public PointV2 direction { get; set; }
        public byte index { get; set; }         
        public byte[,] ArrangementFigures { get; private set; }
        public MementoStep(PointV2 position, PointV2 direction, byte index)
        { 
            CreateArrangementFigures();
            this.position = position;
            this.direction = direction;
            this.index = index;
        }

       
        private byte TagetFigure(IFigure figure)
        {
            if (figure.side == CellStatus.Player1)
                return figure.id;
            else
                return (byte)(figure.id + 1);
        }

       
        protected void CreateArrangementFigures()
        {
            ArrangementFigures = new byte[8, 8];
            for (byte i = 0; i < ArrangementFigures.GetLength(0); i++)
            {
                for (byte j = 0; j < ArrangementFigures.GetLength(1); j++)
                {
                    if (GameBoard.Instance.Figures.Exists(x => x.position.Equals(new PointV2(i, j))))
                    {
                        IFigure figure = GameBoard.Instance.Figures.Find(x => x.position.Equals(new PointV2(i, j)));

                        ArrangementFigures[i, j] = TagetFigure(figure);
                    }
                    else ArrangementFigures[i, j] = 0;
                }
            }
        }
      
    }
    [Serializable()]
    class HystorySteps : Singleton<HystorySteps>
    {
        public byte CountSteps { get { return (byte)Hystory.Count; } }
       private List<MementoStep> Hystory { get; set; }
        public HystorySteps() : base ()
        {
            this.Hystory = new List<MementoStep>();
        }
        public void SaveStep(PointV2 position ,PointV2 direction,CellStatus side)
        {
            Hystory.Add(new MementoStep(position, direction, (byte) (Hystory.Count + 1)));
        }   
          
    }
    //    // АИ, Опонент.
    //    private Player AiPlayer,   EnemyPlayer; 
    //    // Хранит количество ходов, доступных каждой фигуре игрока
    //    private int _amoutStepsPlayer;
    //    // Хранит количество ходов, доступных для удара.
    //    private int _amoutCellsForAttacks;
    //    // Разница в количестве ходов до и после хода фигуры(х)
    //    private int differenceAmoutStep;


    //    // Если количество доступных ходов увеличиться с текущего момента.
    //    private bool IncreaseStepsPlayer ()
    //    {
    //        if (_amoutStepsPlayer == CountSteps())
    //        {
    //            differenceAmoutStep = CountSteps() - _amoutStepsPlayer;
    //            _amoutStepsPlayer = CountSteps();
    //            return true;
    //        }
    //        else
    //        {
    //            differenceAmoutStep = 0;
    //            return false;
    //        }
    //    }
    //    // Если количество ходов для увеличиться с текущего момента.
    //    private bool IncreasePoisitionsForAtack()
    //    {
    //        if (_amoutCellsForAttacks == CountPoisitionsForAtack())
    //        {
    //            differenceAmoutStep = CountPoisitionsForAtack() - _amoutStepsPlayer;
    //            _amoutCellsForAttacks = CountPoisitionsForAtack();
    //            return true;
    //        }
    //        else
    //        {
    //            differenceAmoutStep = 0;
    //            return false;
    //        }
    //    }
    //    // Считает общее количество ходов для атаки.
    //    private int  CountPoisitionsForAtack()
    //    {
    //        int result = 0;
    //        foreach (var item in AiPlayer.Figures)
    //        {
    //            result += item.AmoutCellsForAttacks;
    //        }
    //        return result;
    //    }
    //    // Количество доступных ходов
    //    private int CountSteps()
    //    {
    //        int result = 0;
    //        foreach (var item in AiPlayer.Figures)
    //        {
    //            result += item.AmoutCellsForWalks;
    //        }
    //        return result;
    //    }


    //    public ArtificialIntelligencePlayer(Player AiPlayer, Player EnemyPlayer)
    //    {
    //        this.AiPlayer = AiPlayer;
    //        this.EnemyPlayer = EnemyPlayer;
   
            
    //    }

    ///// <summary>
    // /// Отценка открытия ходов союзным фигурам
    ///// </summary>
    ///// <param name="dir"></param>
    ///// <returns></returns>
    //    private RatedStep EstimateOpenedStepsAllies(PointV2 dir, Player self)
    //    {
    //        RatedStep conditionOfStep = new RatedStep(dir, 0);    
    //        if (IncreasePoisitionsForAtack())
    //            conditionOfStep.Add(differenceAmoutStep);
    //        if (IncreaseStepsPlayer())
    //            conditionOfStep.Add(differenceAmoutStep);
    //        return conditionOfStep;
    //    }
    //    /// <summary>
    //    /// Отценка открытия ходов союзным фигурам
    //    /// </summary>
    //    /// <param name="dir"></param>
    //    /// <returns></returns>
    //    private RatedStep EstimateOpenedStepsAllies(RatedStep conditionOfStep, Player self)
    //    { 
    //        if (IncreasePoisitionsForAtack())
    //            conditionOfStep.Add(differenceAmoutStep);
    //        if (IncreaseStepsPlayer())
    //            conditionOfStep.Add(differenceAmoutStep);
    //        return conditionOfStep;
    //    }

    //    /// <summary>
    //    /// Проверяет находиться ли клетка под боем. Возвращает коофициент утери.
    //    /// </summary>
    //    /// <param name="f"></param>
    //    /// <returns></returns>
    //    private RatedStep PositionIsNotUnderAttack(PointV2 dir, Player enemy,float weight)
    //    {
    //        // Параметры: позиция хода, опонент, вес фигуры для которой вызываеться метод.
    //        // изначально вес шага - 0.
    //        // переберает все варинты атак на данную позицию, и от результата отнимает вес фигуры.
    //        RatedStep conditionOfStep_result = new RatedStep(dir, 0);
    //        foreach (var figureItem in enemy.Figures) 
    //            foreach (var item in figureItem.CellsForWalks)
    //            {
    //                if (dir.Equals(item))
    //                {
    //                    conditionOfStep_result.Subtraction(weight);
    //                }
    //            }
    //        return conditionOfStep_result;
    //    }
    //    private RatedStep PositionIsNotUnderAttack(RatedStep conditionOfStep, Player enemy, float weight)
    //    {
    //        // Параметры: позиция хода, опонент, вес фигуры для которой вызываеться метод.
    //        // изначально вес шага - 0.
    //        // переберает все варинты атак на данную позицию, и от результата отнимает вес фигуры.
             
    //        foreach (var figureItem in enemy.Figures)
    //            foreach (var item in figureItem.CellsForWalks)
    //            {
    //                if (conditionOfStep.Position.Equals(item))
    //                {
    //                    conditionOfStep.Subtraction(weight);
    //                }
    //            }
    //        return conditionOfStep;
    //    }



    //    private void EstimateSelfStep()
    //    {
    //        List<RatedStep> ConditionOfStepList = new List<RatedStep>();
    //        MementoFiguresState mementoFiguresState = new MementoFiguresState();
    //        RatedStep tmp_ConditionOfStep;
    //        foreach (var figure in AiPlayer.Figures)
    //        {
    //            mementoFiguresState.Save(figure);
    //            foreach (var item in figure.CellsForWalks)
    //            {


    //                //tmp_ConditionOfStep = this.PositionIsNotUnderAttack(tmp_ConditionOfStep, EnemyPlayer, figure.id);
    //                //figure.Walk(item);
    //                  //tmp_ConditionOfStep = this.EstimateOpenedStepsAllies();
    //                mementoFiguresState.Return(figure);
    //            }
    //        }

    //    }
        
       
    //  //  /// <summary>
    //  //  /// Отценка открытых шагов союзникам
    //  //  /// </summary>
    //  //  /// <param name="dir"></param>
    //  //  /// <returns></returns>
    //  //private float EstimateOpenedOtherSteps(Player other)
    //  //{
    //  //    float result = 0;
    //  //    return result;
    //  //}
      


          
    //}

    //class RatedFigure
    //{
    //    public RatedFigure (IFigure figure,PointV2 stepPosition, float weight)
    //    {
    //        this.weight = weight;
    //        this.Position = stepPosition;
    //        this.figure = figure;
    //    }

    //    public void Subtraction(float weight)
    //    {
    //        weight -= weight;
    //    }
    //    public void Add(float weight)
    //    {
    //        weight += weight;
    //    }
    //    public IFigure figure { get;private set; }
    //    public PointV2 Position { get;private set; }
    //    public float weight { get;private set; }
    //}
    //class RatedStep
    //{
    //    public RatedStep(PointV2 stepPosition, float weight)
    //    {
    //        this.weight = weight;
    //        this.Position = stepPosition;
    //    }

    //    public void Subtraction(float weight)
    //    {
    //        weight -= weight;
    //    }
    //    public void Add(float weight)
    //    {
    //        weight += weight;
    //    }
    //    public PointV2 Position { get; set; }
    //    public float weight { get; set; }
    //}

    ////class MementoFiguresState  
    ////{
    //// private   List<IFigure> Mementofigures {get;set;}
    //// private List<IFigure> tmp_EmulationFigures {get;set;}


    ////    public void Save (Figure figure)
    ////    {
    ////        // сохраняем состояние фигуры
    ////        Mementofigures.Add(figure.Clone());  
    ////        // добавляем ссылку на фигуру в список.
    ////        tmp_EmulationFigures.Add(figure );   
    ////    }
        
          
    ////    public void ReturnStateFigure (IFigure figure)
    ////    {
    ////        int index = tmp_EmulationFigures.FindIndex(x=>x.Equals(figure));
    ////        tmp_EmulationFigures[index] = Mementofigures[index];
    ////        tmp_EmulationFigures.RemoveAt(index);
    ////        Mementofigures
    ////    } 
    ////     public void ReturnStateFigures ()
    ////    {

    ////    }

    ////}

    //class MementoFiguresState  : IFigure
    //{  
    //    public CellStatus side {get;private set;} 
    //    public PointV2 position{get;private set;} 
    //    public int id{get;set;}
    //    public string name{get;private set;} 
    //    public List<PointV2> CellsForWalks{get;set;} 
    //    public List<PointV2> CellsForAttacks{get;set;} 
    //    public void Save (IFigure link)
    //    {
    //        side = link.side;
    //        position = link.position;
    //        id = link.id;
    //        name = link.name;
    //    }
    //     public void Return (IFigure link)
    //     {
    //         link.side =side;
    //         link.position =position;
    //         link.id = id;
    //        link.name = name;
    //     }
    //}
}
