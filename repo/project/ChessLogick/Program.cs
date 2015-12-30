//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text; 

//namespace Chess
//{
//    class Test
//    {

//        static private void test_pawn()
//        { 
//            Pawn tmp = new Pawn(CellStatus.Player1, new PointV2(2, 0));
//            tmp.DontActioEvent += tmp_testEvent;
//            GameBoard.Instance.Figures.Add((Figure)tmp);
//            GameBoard.Instance.Figures.Add(new Pawn(CellStatus.Player2, new PointV2(3, 1)));

//            tmp.FocusFigureEvent += tmp_FocusFigureEvent;

//            System.Console.ReadKey();
//            tmp.Focus();
//            System.Console.ReadKey();

//        }

//        static void tmp_FocusFigureEvent(IFocusFigure focusFigure)
//        {
//            try
//            {
//                System.Console.WriteLine(focusFigure.id);

//                System.Console.WriteLine(focusFigure.name);

//                System.Console.WriteLine(focusFigure.position);

//                System.Console.WriteLine(focusFigure.side);
//                System.Console.WriteLine();
//                foreach (var item in focusFigure.CellsForWalks)
//                {
//                    System.Console.Write(item);
//                }
//            }
//            catch (System.Exception ex)
//            {
//                System.Console.WriteLine(ex);
//            }
//        }

//        static void tmp_testEvent(string str)
//        {
//            System.Console.WriteLine(str);
//        }



//        static void Main(string[] args)
//        {
//            Bishop bp = new Bishop(CellStatus.Player1, new PointV2(4, 4)); 
            
 
          
//            //int u = 0;
//            //System.Console.WriteLine("result================ " + p.Count);

//            //foreach (var item in p)
//            //{
//            //    u++;
//            //    System.Console.WriteLine(item);
//            //    if (u > 7)
//            //    {
//            //        Console.WriteLine();
//            //        u = 0;
//            //    }
//            //}
//            //System.Console.WriteLine("=====================");
//            Console.ReadKey();
//        }
//    }
//}
