using System;
using System.Linq;
using UnityEngine;

namespace Aniz.Data.Assist
{
    public class DataParserAssist
    {
        static public uint UintParser(string s)
        {
            if (String.IsNullOrEmpty(s))
                return 0;

            uint data;
            uint.TryParse(s, out data);
            return data;
        }

        static public uint HexParser(string s)
        {
            if (String.IsNullOrEmpty(s))
                return 0;

            uint data;
            uint.TryParse(s, System.Globalization.NumberStyles.HexNumber, null, out data);
            return data;
        }

        static public float FloatParser(string s, float value = 0.0f)
        {
            if (String.IsNullOrEmpty(s))
                return 0;

            float data = value;
            float.TryParse(s, out data);
            return data;
        }

        static public int IntParser(string s)
        {
            if (String.IsNullOrEmpty(s))
                return 0;

            int data = 0;
            int.TryParse(s, out data);
            return data;
        }

        static public byte ByteParser(string s)
        {
            if (String.IsNullOrEmpty(s))
                return 0;

            byte data = 0;
            byte.TryParse(s, out data);
            return data;
        }

        static public bool BoolParser(string s)
        {
            if (String.IsNullOrEmpty(s))
                return false;

            bool data;
            bool.TryParse(s, out data);
            if (data)
            {
                return true;
            }
            return false;
        }

        static public ushort UShortParser(string s)
        {
            if (String.IsNullOrEmpty(s))
                return 0;

            ushort data = 0;
            ushort.TryParse(s, out data);

            return data;
        }

        static public Vector2 Vector2Parser(string s)
        {
            if (String.IsNullOrEmpty(s))
                return new Vector2(0.0f, 0.0f);

            if (s.Length <= 0 && s == "")
                return new Vector2(0.0f, 0.0f);

            string[] arraystr = s.Split(',');
            if (arraystr.Length >= 2)
            {
                float x = FloatParser(arraystr[0]);
                float y = FloatParser(arraystr[1]);

                return new Vector2(x, y);
            }
            return new Vector2(0.0f, 0.0f);
        }

        static public Vector3 Vector3Parser(string s)
        {
            if (String.IsNullOrEmpty(s))
                return new Vector3(0.0f, 0.0f);

            string[] arraystr = s.Split(',');
            if (arraystr.Length >= 3)
            {
                float x = FloatParser(arraystr[0]);
                float y = FloatParser(arraystr[1]);
                float z = FloatParser(arraystr[2]);

                return new Vector3(x, y, z);
            }
            return new Vector3(0.0f, 0.0f);
        }
    }
}