using UnityEngine;

namespace Aniz.Graph
{
    public static class V2Ext
    {
        public static Vector3 ToXZ(this Vector2 v)
        {
            return new Vector3(v.x, 0f, v.y);
        }

        public static float Cross(Vector2 a, Vector2 b)
        {
            return a.x * b.y - a.y * b.x;
        }

        public static Vector2 Cross(Vector2 a, float s)
        {
            return new Vector2(s * a.y, -s * a.x);
        }

        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

            float tx = v.x;
            float ty = v.y;
            v.x = (cos * tx) - (sin * ty);
            v.y = (sin * tx) + (cos * ty);
            return v;
        }
    }

    public static class V3Ext
    {
        public static Vector2 XY(this Vector3 v)
        {
            return new Vector2(v.x, v.y);
        }

        public static Vector2 XZ(this Vector3 v)
        {
            return new Vector2(v.x, v.z);
        }

        public static Vector2 YZ(this Vector3 v)
        {
            return new Vector2(v.y, v.z);
        }

        public static Vector2 YX(this Vector3 v)
        {
            return new Vector2(v.y, v.x);
        }

        public static Vector2 ZX(this Vector3 v)
        {
            return new Vector2(v.z, v.x);
        }

        public static Vector2 ZY(this Vector3 v)
        {
            return new Vector2(v.z, v.y);
        }
    }

    public static class Ut // Util
    {
        public static readonly int INVALID_INT = -1;
        public static readonly float INVALID_FLOAT = Mathf.Infinity;
        public static readonly Vector3 INVALID_V3 = new Vector3(INVALID_FLOAT, INVALID_FLOAT, INVALID_FLOAT);
        public static readonly Quaternion INVALID_QUAT = new Quaternion(INVALID_FLOAT, INVALID_FLOAT, INVALID_FLOAT, INVALID_FLOAT);

        public static bool NearlyEqual(float a, float b, float threashold = 0.0001f)
        {
            return (Mathf.Abs(a - b) < threashold);
        }

        public static void Vector3ToString(string prefix, Vector3 v3, ref string output)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("{0}=({1}, {2}, {3})", prefix, v3.x, v3.y, v3.z);
            output = sb.ToString();
        }

        public static void QuaternionToString(string prefix, Quaternion q, ref string output)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("{0}=({1}, {2}, {3}, {4})", prefix, q.x, q.y, q.z, q.w);
            output = sb.ToString();
        }
    }
}