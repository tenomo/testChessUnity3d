using System;
using System.Collections.Generic;


namespace Chess
{
    /// <summary>
    /// Основные алгоритмы движения.
    /// </summary>
    class ManagerAlgorithmsMotion
    {

        /// <summary>
        /// Возвращает список доступных клеток относительно текущей позиции фигуры, в указаном направлении.
        /// </summary>
        /// <param name="figure"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        static private List<PointV2> BustPoints(IMotionInterfaceFigure figure, PointV2 dir)
        {
            // Временно значение точки изменяемое каждую итерацию.
            PointV2 BufferPoint,
                // Позиция фигуры относительно которой вычисляються точки.
                position = figure.position;

            List<PointV2> result = new List<PointV2>();

            // Вычисление начальной точки, с которой будет вестись последующие вычисления.
            // Позиция фигуры не учитываеться !!!
            BufferPoint = position + dir;

            // Все вычисление ведутся пока точка находиться в пределах игровой доски.
            while (GameBoard.Instance.isOnTheChessboard(BufferPoint))
            {

                // Еслм точка выходит за границу доски - выход из цыкла.
                if (!GameBoard.Instance.isOnTheChessboard(BufferPoint))
                    break;

                // Если фигуре доступен шаг на текушую точку 'BufferPoint', добавляем в список-результат.
                // Интерфейс IMotionInterfaceFigure предоставляет метод который содержит каждая фигура
                // и который определяет доступна ли точка для шага данной фигуры.
                if (figure.GetIsAvailableForActions(BufferPoint))
                { 
                    result.Add(BufferPoint);
                    // Шаг в заданом направлении.
                    BufferPoint += dir;
                }
                // Если точка недоступна, все последующие так же не доступны по правилам шахмат.
                // выход из цыкла.
                // Данное правило обходит только фигуру'конь'.
                else
                    break;
            }

            return result;
        }

        /// <summary>
        /// Возвращает список доступных клеток в один шаг, относительно текущей позиции фигуры, в указаном направлении.
        /// </summary>
        /// <param name="figure"></param>
        /// <param name="dir"></param>
        /// <param name="isOneStep"></param>
        /// <returns></returns>
        static private List<PointV2> BustPoints(IMotionInterfaceFigure figure, PointV2 dir, bool isOneStep)
        {
            // Временно значение точки изменяемое каждую итерацию.
            PointV2 BufferPoint,
                // Позиция фигуры относительно которой вычисляються точки.
                position = figure.position;

            List<PointV2> result = new List<PointV2>();

            //Вычисление начальной точки, с которой будет вестись последующие вычисления.
            BufferPoint = position + dir;

            // Если фигуре доступен шаг на текушую точку 'BufferPoint', добавляем в список-результат.
            // Интерфейс IMotionInterfaceFigure предоставляет метод который содержит каждая фигура
            // и который определяет доступна ли точка для шага данной фигуры.
            if (figure.GetIsAvailableForActions(BufferPoint))
            { 
                result.Add(BufferPoint);
            }
            // Данная функция возвращает по сути лишь одну точку, один шаг. 
            // Поэтому возвращаем результат, после вычисления.

            return result;
        }

        /// <summary>
        ///  Возвращает список  клеток по диагонале А - прямая линия относительно текущей позиции состоящая из множества клеток, 
        /// соединющих углы (нижний левый - верхний парвый) условной матрицы , которой являеться игровое поле.
        /// </summary>
        /// <param name="figure"></param>
        /// <param name="isOneStep"></param>
        /// <returns></returns>
        public static List<PointV2> GetDiagonaleA(IMotionInterfaceFigure figure, bool isOneStep)
        {
            #region messy code
            //List<PointV2> result = new List<PointV2>();

            //PointV2 BufferPoint = new PointV2();
            //// Длинна диагонали
            //int diagonaleLenght = 0;


            //// Вычисление  diagonaleLenght
            //if (position.x < position.y)
            //    diagonaleLenght = GameBoard.Instance.height + position.x - position.y;
            //else if (position.x > position.y & position.x != position.y)
            //    diagonaleLenght = GameBoard.Instance.height + position.y - position.x;
            //else diagonaleLenght = GameBoard.Instance.height;


            //// Если х <=  у параметра position, начальная точка ( 1 по х, у   длинна диагонали). 
            ////  if (position.x <= position.y)
            //BufferPoint = new PointV2(GameBoard.Instance.zero, diagonaleLenght);
            //// Иначе  начальная точка  ( ширина игровой доски - (длинна диагонали - 1), у 1). 
            //// Тоесть, начальная точка что лежит на побочной диагонали, если! она ниже основной. 
            ////                            НЕОБХОДИМО ТЕСТИРОВАТЬ.
            ////  else
            /////     BufferPoint = new PointV2(GameBoard.Instance.width - (diagonaleLenght - 1), GameBoard.Instance.zero);


            //while (GameBoard.Instance.isOnTheChessboard(BufferPoint))
            //{
            //    result.Add(BufferPoint);
            //    BufferPoint += PointV2.up + PointV2.right;
            //    if (!GameBoard.Instance.isOnTheChessboard(BufferPoint))
            //        break;
            //}


            //return result;
            #endregion

            // FIX
            // Направление вниз и влево.
            PointV2 dirDownAndLeft = (PointV2.down + PointV2.left),
                // Направление вверх и вправо
                dirUpAndright = (PointV2.up + PointV2.right);

            List<PointV2> result = new List<PointV2>();
            // Добавляет к результату, множества точек, относительно заданых параметров.

            if (!isOneStep)
            {
                result.AddRange(BustPoints(figure, dirDownAndLeft));
                result.AddRange(BustPoints(figure, dirUpAndright));
            }
            // Добавляет к результату один шаг, относительно заданых параметров.
            else
            {
                result.AddRange(BustPoints(figure, dirDownAndLeft, isOneStep));
                result.AddRange(BustPoints(figure, dirUpAndright, isOneStep));
            } 
            return result;
        }

        /// <summary>
        ///  Возвращает список  клеток по диагонале А - прямая линия относительно текущей позиции состоящая из множества клеток, 
        /// соединющих углы (верхний левый - нижний парвый) условной матрицы, которой являеться игровое поле.
        /// </summary>
        /// <param name="figure"></param>
        /// <param name="isOneStep"></param>
        /// <returns></returns>
        public static List<PointV2> GetDiagonaleB(IMotionInterfaceFigure figure, bool isOneStep)
        {
            #region messy code
            //List<PointV2> result = new List<PointV2>();

            //PointV2 BufferPoint = new PointV2();
            //// Длинна диагонали
            //int diagonaleLenght = 0;


            //// Вычисление  diagonaleLenght
            //if (position.x + position.y > GameBoard.Instance.width + 1)
            //    diagonaleLenght = 2 * GameBoard.Instance.width - position.x - position.y + 1;
            //else if (position.x + position.y < GameBoard.Instance.width)
            //    diagonaleLenght = position.x + position.y - 1;


            //// Если х <=  у параметра position, начальная точка ( 1 по х, у   длинна диагонали).          
            //if (position.x + position.y >= GameBoard.Instance.width)
            //    BufferPoint = new PointV2(GameBoard.Instance.zero, diagonaleLenght);
            //else
            //{
            //    // Иначе  начальная точка  ( ширина игровой доски - (длинна диагонали - 1), у height). 
            //    // Тоесть, начальная точка что лежит на побочной диагонали, если! она ниже основной. 
            //    //              НЕОБХОДИМО ТЕСТИРОВАТЬ.
            //    BufferPoint = new PointV2(GameBoard.Instance.width - (diagonaleLenght - 1), GameBoard.Instance.height);
            //}

            //while (GameBoard.Instance.isOnTheChessboard(BufferPoint))
            //{
            //    result.Add(BufferPoint);
            //    BufferPoint += PointV2.down + PointV2.right;
            //    if (!GameBoard.Instance.isOnTheChessboard(BufferPoint))
            //        break;
            //}



            //return result;
            //===============================================================================================
            //=============================== FIX ==========================================================
            // =============================================================================================
            #endregion

            // Направление вверх и вправо.
            PointV2 dirDownAndRight = (PointV2.down + PointV2.right),
                // Направление вниз и влево.
                stepUp = (PointV2.up + PointV2.left);

            List<PointV2> result = new List<PointV2>();
            // Добавляет к результату, множества точек, относительно заданых параметров.

            if (!isOneStep)
            {
                result.AddRange(BustPoints(figure, dirDownAndRight));
                result.AddRange(BustPoints(figure, stepUp));
            }
            // Добавляет к результату один шаг, относительно заданых параметров.
            else
            {
                result.AddRange(BustPoints(figure, dirDownAndRight, isOneStep));
                result.AddRange(BustPoints(figure, stepUp, isOneStep));
            } 
            return result;
        }

        /// <summary>
        /// <para> Вовзращает список клеток по вертекале. Если значении isOneStep равняеться true,  список с одним  </para>
        /// <para>доступным элементом, если значение равняеться false, все доступные элементы по диагонале.</para>
        /// </summary>
        /// <param name="figure"></param>
        /// <param name="isOneStep"></param>
        /// <returns></returns>
        public static List<PointV2> GetVertical(IMotionInterfaceFigure figure, bool isOneStep)
        {
            #region messy code
            //PointV2 bufferPoint = new PointV2(figure.position.x, GameBoard.Instance.zero);
            //result.Add(bufferPoint);
            //while (GameBoard.Instance.isOnTheChessboard(bufferPoint))
            //{
            //    if ()
            //    bufferPoint += PointV2.up;
            //    result.Add(bufferPoint);
            //    if (!GameBoard.Instance.isOnTheChessboard(bufferPoint))
            //        break;                
            //}
            #endregion

            List<PointV2> result = new List<PointV2>();

            // Добавляет к результату, множества точек, относительно заданых параметров.
            if (!isOneStep)
            {
                result.AddRange(BustPoints(figure, PointV2.up));
                result.AddRange(BustPoints(figure, PointV2.down));
            }
            // Добавляет к результату один шаг, относительно заданых параметров.
            else
            {
                result.AddRange(BustPoints(figure, PointV2.up, isOneStep));
                result.AddRange(BustPoints(figure, PointV2.down, isOneStep));
            }
            return result;
        }

        /// <summary>
        ///  <para>Возвращает спискок клеток по горизонтале. Если значении isOneStep равняеться true,  список с одним  </para>
        ///  <para>доступным элементом, если значение равняеться false, все доступные элементы по диагонале.</para>
        /// </summary>
        /// <param name="figure"></param>
        /// <param name="isOneStep"></param>
        /// <returns></returns>
        public static List<PointV2> GetHorisontal(IMotionInterfaceFigure figure, bool isOneStep)
        {
            #region messy code
            //PointV2 bufferPoint = new PointV2(GameBoard.Instance.zero, thisPosition.y);
            //result.Add(bufferPoint);
            //while (GameBoard.Instance.isOnTheChessboard(bufferPoint))
            //{
            //    bufferPoint += PointV2.right;
            //    if (!GameBoard.Instance.isOnTheChessboard(bufferPoint))
            //        break;
            //    result.Add(bufferPoint);
            //}
            #endregion

            List<PointV2> result = new List<PointV2>();

            // Добавляет к результату, множества точек, относительно заданых параметров.
            if (!isOneStep)
            {
                result.AddRange(BustPoints(figure, PointV2.left));
                result.AddRange(BustPoints(figure, PointV2.right));
            }
            // Добавляет к результату один шаг, относительно заданых параметров.
            else
            {
                result.AddRange(BustPoints(figure, PointV2.left, isOneStep));
                result.AddRange(BustPoints(figure, PointV2.right, isOneStep));
            } 
            return result;
        }
    }
}
