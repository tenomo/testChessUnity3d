using System;
using System.Collections.Generic;
 

namespace Chess
{
    public struct PointV2
    {
        private int _X, _Y;
        public int x
        {
            get { return _X; }
            set { _X = value; }
        }
        public int y
        {
            get { return _Y; }
            set { _Y = value; }

        }
        

        public PointV2 (int x, int y)
        {
            _X = x;
            _Y = y;
        }
        /// <summary>
        /// Перегрузка операторов для структуры PointV2. Позволяет вполняет арефметические, логические действия по отношениб
        /// к экземлярам структуры PointV2
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
#region operator overloading
        public static PointV2 operator +(PointV2 p1, PointV2 p2)
        {
            return new PointV2(p1.x + p2.x, p1.y + p2.y);
        }
        public static PointV2 operator +(PointV2 p1, int n)
        {
            return new PointV2(p1.x + n, p1.y + n);
        }
        public static PointV2 operator /(PointV2 p1, PointV2 p2)
        {
            return new PointV2(p1.x / p2.x, p1.y / p2.y);
        }
        public static PointV2 operator /(PointV2 p1, int n)
        {
            return new PointV2(p1.x / n, p1.y / n);
        }
        public static PointV2 operator *(PointV2 p1, PointV2 p2)
        {
            return new PointV2(p1.x * p2.x, p1.y * p2.y);
        }
        public static PointV2 operator *(PointV2 p1, int n)
        {
            return new PointV2(p1.x * n, p1.y * n);
        }
        public static PointV2 operator -(PointV2 p1, int n)
        {
            return new PointV2(p1.x - n, p1.y - n);
        }
        public static PointV2 operator -(PointV2 p1, PointV2 p2)
        {
            return new PointV2(p1.x - p2.x, p1.y - p2.y);
        }

        public static PointV2 operator -(PointV2 p1)
        {
            return p1 * -1;
        }
#endregion operator overloading

        /// <summary>
        /// Сравнивает экземпляры структуры  PointV2, по значению.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            PointV2 tmp = (PointV2)obj;
            if (x == tmp.x && y == tmp.y)
                return true;
            else
                return false;
        }

        public override string ToString()
        {
            return "PointV2(" + x + "," + y + ")";
        }


       /// <summary>
        /// PointV2(1, 0)
       /// </summary>
        public static PointV2 right { get { return new PointV2(1, 0); } }
        /// <summary>
        /// PointV2(-1, 0)
        /// </summary>
        public static PointV2 left { get { return new PointV2(-1, 0); } }
        /// <summary>
        /// PointV2(0, 1)
        /// </summary>
        public static PointV2 up { get { return new PointV2(0, 1); } }
        /// <summary>
        /// PointV2(0, -1)
        /// </summary>
        public static PointV2 down { get { return new PointV2(0, -1); } }

        private int Normalize(int n)
        {  
            if (n > 0)
                return  1;
            else if (n < 0)
                return -1;
        return 0;
        }
       /// <summary>
        /// Gреобразование заданного вектора в вектор в том же направлении, но с единичной длиной.
       /// </summary>
       /// <returns></returns>
       public PointV2 Normalize()
        {
            this.x = Normalize(x);
            this.y = Normalize(y);
            return this;
        }
        // НЕ АКТУАЛЬНО ЗА СЧЕТ ПЕРЕГРУЗКИ ОПЕРАТОРОВ.
        // костыль
        ///// <summary>
        ///// Трансформирует текущую координату, меняя значение каждой оси на противоположную.
        ///// </summary>
        //public void GetOppositePoint ()
        //{
        //    this = -this;
        //}
    }
}
