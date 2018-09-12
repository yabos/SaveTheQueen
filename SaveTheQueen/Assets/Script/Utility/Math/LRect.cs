using System;
using System.IO;
using System.Xml;
using System.ComponentModel;
using System.Collections.Generic;

namespace Aniz.Basis
{
    [TypeConverter(typeof(TRectConverter))]
    public class LRect
    {
        private int left;
        private int top;
        private int right;
        private int bottom;

        public static LRect None = new LRect(0, 0, 0, 0);
        public static LRect Load(XmlNode child)
        {
            int x = int.Parse(child.Attributes["left"].Value);
            int y = int.Parse(child.Attributes["top"].Value);
            int width = int.Parse(child.Attributes["width"].Value);
            int height = int.Parse(child.Attributes["height"].Value);
            return new LRect(x, y, width, height);
        }

        public LRect()
        {
            this.left = 0;
            this.top = 0;
            this.right = 0;
            this.bottom = 0;
        }

        //         public LRect(int left, int top, int right, int bottom)
        //         {
        //             this.left = left;
        //             this.top = top;
        //             this.right = right;
        //             this.bottom = bottom;
        //         }

        public LRect(int x, int y, int w, int h)
        {
            left = x;
            top = y;
            this.right = left + w;
            this.bottom = top + h;
        }

        public LRect(LPoint position, int width, int height)
        {
            left = position.X;
            top = position.Y;
            this.right = left + width;
            this.bottom = top + height;
        }

        public LRect(LPoint pointA, LPoint pointB)
        {
            left = Math.Min(pointA.X, pointB.X);
            top = Math.Min(pointA.Y, pointB.Y);
            right = Math.Max(pointA.X, pointB.X);
            bottom = Math.Max(pointA.Y, pointB.Y);
        }

        public static bool operator ==(LRect lhs, LRect rhs)
        {
            if ((lhs.left == rhs.left) &&
                (lhs.top == rhs.top) &&
                (lhs.right == rhs.right) &&
                (lhs.bottom == rhs.bottom))
            {
                return true;
            }
            return false;
        }

        public static bool operator !=(LRect lhs, LRect rhs)
        {
            return !(lhs == rhs);
        }

        public static LRect operator *(LRect lhs, int rhs)
        {
            return new LRect(lhs.Position, lhs.Width * rhs, lhs.Height * rhs);
        }

        public bool Has(LPoint point)
        {
            return (left <= point.X && top <= point.Y) && (left + Width > point.X && top + Height > point.Y);
        }

        public bool Include(LRect rect)
        {
            if ((rect.left >= Right) || (rect.Right <= left)) return false;

            if ((rect.Bottom <= top) || (rect.top >= Bottom)) return false;

            return true;
        }

        [Browsable(false)]
        public LPoint Position
        {
            get { return new LPoint(left, top); }
        }

        [Browsable(false)]
        public LPoint Size
        {
            get { return new LPoint(Width, Height); }
        }

        [Browsable(false)]
        public LPoint RightBottom
        {
            get { return new LPoint(right, bottom); }
        }


        public void SetLeft(int nleft)
        {
            left = nleft;
        }

        public int Left
        {
            get { return left; }
            set { left = value; }
        }

        public int Top
        {
            get { return top; }
            set { top = value; }
        }

        public int Right
        {
            get { return right; }
            set { right = value; }
        }

        public int Bottom
        {
            get { return bottom; }
            set { bottom = value; }
        }


        public int x
        {
            get { return left; }
            set { left = value; }
        }

        public int y
        {
            get { return top; }
            set { top = value; }
        }

        public int w
        {
            get { return right; }
            set { right = value; }
        }

        public int h
        {
            get { return bottom; }
            set { bottom = value; }
        }

        [Browsable(false)]
        public int Width
        {
            get { return right - left; }
            set { right = left + value; }
        }

        [Browsable(false)]
        public int Height
        {
            get { return bottom - top; }
            set { bottom = top + value; }
        }


        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3}", left, top, right, bottom);
        }

        public static LRect Join(LRect lhs, LRect rhs)
        {
            LPoint start = new LPoint(Math.Min(lhs.left, rhs.left), Math.Min(lhs.top, rhs.top));
            LPoint end = new LPoint(Math.Max(lhs.Right, rhs.Right), Math.Max(lhs.Bottom, rhs.Bottom));
            return new LRect(start, end);
        }

        #region For Disable Warning

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return x * 131071 ^ w * 3571 ^ y * 3109 ^ h * 7;
        }

        public LRect Inflate(int distance)
        {
            return new LRect(x - distance, y - distance,
              Width + (distance * 2), Height + (distance * 2));
        }

        public static LRect Column(int x, int y, int size)
        {
            return new LRect(new LPoint(x, y), 1, size);
        }

        public static LRect Row(int x, int y, int size)
        {
            return new LRect(new LPoint(x, y), size, 1);
        }

        public List<LPoint> ToList()
        {
            List<LPoint> result = new List<LPoint>();
            for (int ix = x; ix < x + Width; ix++)
            {
                for (int iy = y; iy < y + Height; iy++)
                {
                    result.Add(new LPoint(ix, iy));
                }
            }
            return result;
        }

        public List<LPoint> Trace()
        {
            if ((Width > 1) && (Height > 1))
            {
                // TODO(bob): Implement an iterator class here if building the list is
                // slow.
                // Trace all four sides.
                List<LPoint> result = new List<LPoint>();

                for (var x = left; x < right; x++)
                {
                    result.Add(new LPoint(x, top));
                    result.Add(new LPoint(x, bottom - 1));
                }

                for (var y = top + 1; y < bottom - 1; y++)
                {
                    result.Add(new LPoint(left, y));
                    result.Add(new LPoint(right - 1, y));
                }

                return result;

            }
            else if ((Width > 1) && (Height == 1))
            {
                // A single row.
                return Row(left, top, Width).ToList();
            }
            else if ((Height >= 1) && (Width == 1))
            {
                // A single column, or one unit
                return Column(left, top, Height).ToList();
            }
            // Otherwise, the rect doesn't have a positive size, so there's nothing to
            // trace.
            return new List<LPoint>();
        }


        #endregion


        public static LRect Parse(string str)
        {
            string[] values = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            if (values.Length != 4)
            {
                throw new FileLoadException("Point 정보가 잘못됨");
            }

            int iLeft = int.Parse(values[0]);
            int iTop = int.Parse(values[1]);
            int iWeight = int.Parse(values[2]);
            int iHeight = int.Parse(values[3]);

            return new LRect(iLeft, iTop, iWeight, iHeight);
        }

        /// <summary>
        /// 상하좌우로 size만큼 커진다
        /// </summary>
        public void Grow(int size)
        {
            left -= size;
            top -= size;
            Width += size * 2;
            Height += size * 2;
        }

        public void Offset(LPoint offset)
        {
            left += offset.X;
            top += offset.Y;
        }

        /// <summary>
        /// 상하좌우로 size만큼 작아진다
        /// </summary>


        public void Resize(LPoint size)
        {
            Width += size.X;
            Height += size.Y;
        }

        /// Creates a new rectangle that is the intersection of [a] and [b].
        ///
        ///     .----------.
        ///     | a        |
        ///     | .--------+----.
        ///     | | result |  b |
        ///     | |        |    |
        ///     '-+--------'    |
        ///       |             |
        ///       '-------------'
        public static LRect Intersect(LRect lhs, LRect rhs)
        {
            int left = Math.Max(lhs.left, rhs.left);
            int right = Math.Min(lhs.Right, rhs.Right);

            int top = Math.Max(lhs.top, rhs.top);
            int bottom = Math.Min(lhs.Bottom, rhs.Bottom);

            //             if (left >= right) return new LRect(0, 0, 0, 0);
            //             if (top >= bottom) return new LRect(0, 0, 0, 0);

            int width = Math.Max(0, right - left);
            int height = Math.Max(0, bottom - top);

            return new LRect(new LPoint(left, top), width, height);
        }
    }

    class TRectConverter : ExpandableObjectConverter
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
                LRect rt = (LRect)value;
                return rt.Left + "," + rt.Top + rt.Right + "," + rt.Bottom;
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
                    return LRect.Parse(s);
                }
                catch { }
                throw new ArgumentException("Can not convert '" + (string)value + "' to type Person");
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}