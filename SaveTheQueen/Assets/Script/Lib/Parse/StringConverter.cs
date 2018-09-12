using System;
using System.Collections.Generic;
using UnityEngine;
using Lib.AnimationEvent;

namespace Lib.Parse
{

    public static class StringConverter
    {

        delegate object StringConverterFunc(string input);

        private static Dictionary<System.Type, StringConverterFunc> conversionMap = null;

        public static void BuildConversionMap()
        {
            conversionMap = new Dictionary<System.Type, StringConverterFunc>();
            conversionMap.Add(typeof(string), ConvertToString);
            conversionMap.Add(typeof(bool), ConvertToBool);
            conversionMap.Add(typeof(float), ConvertToFloat);
            conversionMap.Add(typeof(int), ConvertToInt);
            conversionMap.Add(typeof(uint), ConvertToUInt);
            conversionMap.Add(typeof(long), ConvertToLong);
            conversionMap.Add(typeof(ulong), ConvertToULong);
            conversionMap.Add(typeof(short), ConvertToShort);
            conversionMap.Add(typeof(ushort), ConvertToUShort);

            conversionMap.Add(typeof(Vector3), ConvertToVector3);
            conversionMap.Add(typeof(Quaternion), ConvertToQuaternion);
            conversionMap.Add(typeof(Color), ConvertToColor);

            conversionMap.Add(typeof(eAnimationGameEvent), ConvertToAnimationGameEvent);
            conversionMap.Add(typeof(ePostEffectType), ConvertToPostEffectType);
            conversionMap.Add(typeof(eShaderAnimEventType), ConvertToShaderEffectType);
        }

        public static bool TryConvert<T>(string s, out T output)
        {
            if (conversionMap == null)
            {
                BuildConversionMap();
            }

            StringConverterFunc converter = null;
            if (!conversionMap.TryGetValue(typeof(T), out converter))
            {
                output = default(T);
                return false;
            }

            object value = converter(s);
            output = (T)value;
            return true;
        }

        public static object ConvertToString(string s)
        {
            return s;
        }

        public static object ConvertToBool(string s)
        {
            if (s == "True")
                return true;
            return false;
        }

        public static object ConvertToFloat(string s)
        {
            return float.Parse(s);
        }

        public static object ConvertToInt(string s)
        {
            return int.Parse(s);
        }

        public static object ConvertToUInt(string s)
        {
            return uint.Parse(s);
        }

        public static object ConvertToLong(string s)
        {
            return long.Parse(s);
        }

        public static object ConvertToULong(string s)
        {
            return ulong.Parse(s);
        }

        public static object ConvertToShort(string s)
        {
            return short.Parse(s);
        }

        public static object ConvertToUShort(string s)
        {
            return ushort.Parse(s);
        }




        private static char[] vectorSeparators = new char[] { '|' };

        public static object ConvertToVector3(string s)
        {
            Vector3 result;

            string[] tokens = s.Split(vectorSeparators, System.StringSplitOptions.None);
            if (tokens.Length == 3)
            {
                result.x = float.Parse(tokens[0]);
                result.y = float.Parse(tokens[1]);
                result.z = float.Parse(tokens[2]);
            }
            else
            {
                result = Vector3.zero;
            }
            return result;
        }

        public static object ConvertToQuaternion(string s)
        {
            Quaternion result;

            string[] tokens = s.Split(vectorSeparators, System.StringSplitOptions.None);
            if (tokens.Length == 4)
            {
                result.x = float.Parse(tokens[0]);
                result.y = float.Parse(tokens[1]);
                result.z = float.Parse(tokens[2]);
                result.w = float.Parse(tokens[3]);
            }
            else
            {
                result = Quaternion.identity;
            }
            return result;
        }


        public static object ConvertToColor(string s)
        {
            uint color = uint.Parse(s);
            Color32 c = new Color32((byte)((color >> 24) & 0xff),
                (byte)((color >> 16) & 0xff),
                (byte)((color >> 8) & 0xff),
                (byte)(color & 0xff));
            Color result = c;
            return result;
        }

        public static object ConvertToAnimationGameEvent(string value)
        {
            return GetEnumFromString<eAnimationGameEvent>(value, eAnimationGameEvent.NONE);
            //return System.Enum.Parse(typeof(E_AnimationGameEvent), value, true);
        }

        public static object ConvertToPostEffectType(string value)
        {
            return GetEnumFromString<ePostEffectType>(value, ePostEffectType.NONE);
        }

        public static object ConvertToShaderEffectType(string value)
        {
            return GetEnumFromString<eShaderAnimEventType>(value, eShaderAnimEventType.NONE);
        }

        public static T GetEnumFromString<T>(string val, T defaultValue) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            object obj = val;
            if (System.Enum.IsDefined(typeof(T), obj))
            {
                //return (T)obj;
                return (T)System.Enum.Parse(typeof(T), val, true);
            }
            return defaultValue;
        }


    }
}