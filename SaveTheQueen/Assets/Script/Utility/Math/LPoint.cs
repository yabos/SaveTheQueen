using System;
using System.IO;
using System.ComponentModel;
using UnityEngine;

namespace Aniz.Basis
{
    public struct LPoint : IComparable<LPoint>
    {
        private int m_x;
        private int m_y;

        public static LPoint up = new LPoint(0, 1); //north
        public static LPoint right = new LPoint(1, 0);  //east
        public static LPoint down = new LPoint(0, -1);  //south
        public static LPoint left = new LPoint(-1, 0);  //west

        public static LPoint Zero = new LPoint(0, 0);

        public LPoint(int x, int y)
        {
            m_x = x;
            m_y = y;
        }

        public LPoint(LPoint p)
        {
            m_x = p.X;
            m_y = p.Y;
        }


        public LPoint(Vector2 position)
        {
            m_x = (int)position.x;
            m_y = (int)position.y;
        }

        public LPoint Set(int iX, int iY)
        {
            this.m_x = iX;
            this.m_y = iY;
            return this;
        }

        public bool IsIn(Rect rect)
        {
            return rect.Contains(new Vector2(m_x, m_y));
        }

        public static LPoint MaxValue
        {
            get
            {
                return new LPoint(int.MaxValue, int.MaxValue);
            }
        }

        public static LPoint MinValue
        {
            get
            {
                return new LPoint(int.MinValue, int.MinValue);
            }
        }

        public int X
        {
            get { return m_x; }
            set { m_x = value; }
        }

        public int Y
        {
            get { return m_y; }
            set { m_y = value; }
        }

        public int x
        {
            get { return m_x; }
            set { m_x = value; }
        }

        public int y
        {
            get { return m_y; }
            set { m_y = value; }
        }

        public static LPoint operator -(LPoint lhs)
        {
            return new LPoint(-lhs.X, -lhs.Y);
        }

        public static LPoint operator /(LPoint lhs, int value)
        {
            return new LPoint((int)(lhs.X / value), (int)(lhs.Y / value));
        }

        public static bool operator ==(LPoint lhs, LPoint rhs)
        {
            return (lhs.X == rhs.X) && (lhs.Y == rhs.Y);
        }

        public static bool operator !=(LPoint lhs, LPoint rhs)
        {
            return (lhs.X != rhs.X) || (lhs.Y != rhs.Y);
        }

        public static LPoint operator +(LPoint lhs, LPoint rhs)
        {
            return new LPoint(lhs.X + rhs.X, lhs.Y + rhs.Y);
        }

        public static LPoint operator *(LPoint lhs, float value)
        {
            return new LPoint((int)(value * lhs.X), (int)(value * lhs.Y));
        }

        public static LPoint operator -(LPoint lhs, LPoint rhs)
        {
            return new LPoint(lhs.X - rhs.X, lhs.Y - rhs.Y);
        }


        public void Offset(LPoint p)
        {
            m_x += p.X;
            m_y += p.Y;
        }

        public void Offset(int dx, int dy)
        {
            m_x += dx;
            m_y += dy;
        }


        public static implicit operator Vector2(LPoint From)
        {
            return new Vector2(From.x, From.y);
        }

        public static implicit operator LPoint(Vector2 From)
        {
            return new LPoint((int)From.x, (int)From.y);
        }

        public static implicit operator string(LPoint From)
        {
            return string.Format("({0},{1}", From.x, From.y);
        }

        public override string ToString()
        {
            return string.Format("{0},{1}", X, Y);
        }

        public static float Distance(LPoint curPos, LPoint prevPos)
        {
            return (float)Math.Sqrt(Math.Pow(curPos.X - prevPos.X, 2) + Math.Pow(curPos.Y - prevPos.Y, 2));
        }

        public int sqrMagnitude
        {
            get
            {
                return x * x + y * y;
            }
        }

        public int sqrMagnitudeint
        {
            get
            {
                return (int)x * (int)x + (int)y * (int)y;
            }
        }


        public static int Dot(LPoint a, LPoint b)
        {
            return a.x * b.x + a.y * b.y;
        }

        public static int Dotint(LPoint a, LPoint b)
        {
            return (int)a.x * (int)b.x + (int)a.y * (int)b.y;
        }


        /** Matrices for rotation.
         * Each group of 4 elements is a 2x2 matrix.
         * The XZ position is multiplied by this.
         * So
         * \code
         * //A rotation by 90 degrees clockwise, second matrix in the array
         * (5,2) * ((0, 1), (-1, 0)) = (2,-5)
         * \endcode
         */
        private static readonly int[] Rotations = {
         1, 0, //Identity matrix
		 0, 1,

         0, 1,
        -1, 0,

        -1, 0,
         0,-1,

         0,-1,
         1, 0
        };



        /** Returns a new Int2 rotated 90*r degrees around the origin. */
        public static LPoint Rotate(LPoint v, int r)
        {
            r = r % 4;
            return new LPoint(v.x * Rotations[r * 4 + 0] + v.y * Rotations[r * 4 + 1], v.x * Rotations[r * 4 + 2] + v.y * Rotations[r * 4 + 3]);
        }

        public static LPoint Min(LPoint a, LPoint b)
        {
            return new LPoint(System.Math.Min(a.x, b.x), System.Math.Min(a.y, b.y));
        }

        public static LPoint Max(LPoint a, LPoint b)
        {
            return new LPoint(System.Math.Max(a.x, b.x), System.Math.Max(a.y, b.y));
        }


        #region For Disable Warning

        public override bool Equals(object obj)
        {
            //return base.Equals(obj);
            LPoint p = (LPoint)obj;
            // Return true if the fields match:
            return (X == p.X) && (Y == p.Y);
        }

        public bool Equals(LPoint p)
        {
            // Return true if the fields match:
            return (X == p.X) && (Y == p.Y);
        }

        //         public override int GetHashCode()
        //         {
        //             return (X & 0x0000ffff) << 16 | (Y & 0x0000ffff);
        //         }

        //         public override int GetHashCode()
        //         {
        //             return x * 49157 + y * 98317;
        //         }

        public override int GetHashCode()
        {
            return x ^ y;
        }



        #endregion

        public static LPoint Parse(string str)
        {
            string[] values = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length != 2) throw new FileLoadException("LPoint 정보가 잘못됨");
            return new LPoint(int.Parse(values[0]),
                             int.Parse(values[1]));
        }

        public int CompareTo(LPoint other)
        {
            if (X > other.X) return 1;
            else if (X < other.X) return -1;

            if (Y > other.Y) return 1;
            else if (Y < other.Y) return -1;

            return 0;
        }

    }

    class TPointConverter : ExpandableObjectConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is LPoint)
            {
                LPoint p = (LPoint)value;

                return p.X + "," + p.Y;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                try
                {
                    string s = (string)value;
                    return LPoint.Parse(s);
                }
                catch { }
                throw new ArgumentException("Can not convert '" + (string)value + "' to type Person");
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}