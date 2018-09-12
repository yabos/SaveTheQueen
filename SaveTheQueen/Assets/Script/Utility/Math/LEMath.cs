using System;
using System.Collections.Generic;
using UnityEngine;

public enum E_MathOperation
{
    LessThan,
    LessThanOrEqualTo,
    EqualTo,
    NotEqualTo,
    GreaterThanOrEqualTo,
    GreaterThan
}
public static class LEMath
{

    public static readonly float s_fPI = Mathf.PI;
    public static readonly float s_fHalfPI = s_fPI * 0.5f;
    public static readonly float s_f2PI = s_fPI * 2.0f;
    public static readonly float s_fEpsilon = 0.00001f;
    public static readonly float s_fRadianToDegree = 180.0f / s_fPI;
    public static readonly float s_fDegreeToRadian = s_fPI / 180.0f;

    public const float DirectionEpsilon = 1 / 1024.0f;
    public const float PositionEpsilon = 1 / 128.0f;
    public const float HalfPositionEpsilon = PositionEpsilon / 2.0f;

    public static string[] MathOperationString;

    public static void Init()
    {

        GetMathOperationString();
    }

    public static string[] GetMathOperationString()
    {
        if (MathOperationString == null)
        {
            MathOperationString = new string[(int)(E_MathOperation.GreaterThan) + 1];
            for (int i = 0; i < ((int)E_MathOperation.GreaterThan) + 1; i++)
            {
                E_MathOperation math = (E_MathOperation)(i);
                MathOperationString[i] = string.Format("{0} {1}", OperationString(math), math);
            }
        }

        return MathOperationString;
    }

    public static string OperationString(E_MathOperation eOperation)
    {
        switch (eOperation)
        {
            case E_MathOperation.LessThan:
                return "<";
            case E_MathOperation.LessThanOrEqualTo:
                return "<=";
            case E_MathOperation.EqualTo:
                return "==";
            case E_MathOperation.NotEqualTo:
                return "!=";
            case E_MathOperation.GreaterThanOrEqualTo:
                return ">=";
            case E_MathOperation.GreaterThan:
                return ">";
        }
        return string.Empty;
    }

    public static int Min(int v1, int v2)
    {
        return v1 < v2 ? v1 : v2;
    }

    public static int Max(int v1, int v2)
    {
        return v1 > v2 ? v1 : v2;
    }

    public static int Abs(int v)
    {
        return (v > 0) ? v : -v;
    }

    public static float Abs(float v)
    {
        return (v > 0) ? v : -v;
    }

    public static float Min(float v1, float v2)
    {
        return v1 < v2 ? v1 : v2;
    }

    public static float Max(float v1, float v2)
    {
        return v1 > v2 ? v1 : v2;
    }

    public static void Swap<T>(ref T t1, ref T t2)
    {
        T temp = t1;
        t1 = t2;
        t2 = temp;
    }

    public static int Modulo(int x, int n)
    {
        return ((x % n) + n) % n;
    }

    public static float Modulo(float x, float n)
    {
        return ((x % n) + n) % n;
    }

    public static float SmoothStep(float u)
    {
        return 0.5f - (0.5f * Mathf.Cos(u * 3.141593f));
    }

    public static Vector2 ToCellVector2(Vector3 value)
    {
        return new Vector2(value.x, value.z);
    }

    public static Vector2 ToVectorDir(byte dir)
    {
        return RadianToDir(ByteToRadian(dir));
    }

    public static byte ToByteDir(Vector3 lt)
    {
        return RadianToByte(Radian(lt));
    }

    public static byte ToByteDir(Vector2 lt)
    {
        return RadianToByte(Radian(lt));
    }

    public static float Radian(Vector2 v)
    {
        return Mathf.Atan2(-v.x, -v.y);
    }

    public static float Radian(Vector3 v)
    {
        return Mathf.Atan2(-v.x, -v.z);
    }

    public static Vector2 ThetaToDir(float i_fAngle)
    {
        return new Vector2(-Mathf.Sin(i_fAngle), -Mathf.Cos(i_fAngle));
    }

    public static float RadianToDegree(float i_fRadian)
    {
        return i_fRadian * s_fRadianToDegree;
    }

    public static float DegreeToRadian(float i_fDegree)
    {
        return i_fDegree * s_fDegreeToRadian;
    }

    public static byte RadianToByte(float i_fRadian)
    {
        uint dwValue = (uint)(i_fRadian / s_f2PI * 0x0100);
        return (byte)(dwValue & 0x000000FF);
    }

    public static Vector2 RadianToDir(float fRadian)
    {
        return new Vector2(-Mathf.Sin(fRadian), -Mathf.Cos(fRadian));
    }

    public static Vector3 RadianToDir3(float fRadian)
    {
        return new Vector3(-Mathf.Sin(fRadian), 0.0f, -Mathf.Cos(fRadian));
    }

    public static float ByteToRadian(byte i_byDirection)
    {
        return (float)(i_byDirection) * s_f2PI / 0x0100;
    }

    public static float FMOD(float a, float b)
    {
        return a - b * Mathf.Floor(a / b);
    }

    public static float FABS(float rValue)
    {
        return (rValue > 0) ? rValue : (-rValue);
    }

    public static float RadianIn2PI(float i_fRadian)
    {
        int n2PIMulti = (int)(i_fRadian / s_f2PI);
        float fRadian = i_fRadian - n2PIMulti * s_f2PI;

        if (fRadian < 0.0f)
        {
            return fRadian + s_f2PI;
        }
        return fRadian;
    }

    public static float RadianDistance(float i_fRadian1, float i_fRadian2)
    {
        float fRadianDistance = RadianIn2PI(i_fRadian2) - RadianIn2PI(i_fRadian1);
        return CheckRadian(fRadianDistance);
    }

    public static float CheckRadian(float radian)
    {
        if (radian > s_fPI)
        {
            radian -= s_f2PI;
        }
        else if (radian < -s_fPI)
        {
            radian += s_f2PI;
        }
        return radian;
    }

    public static Vector3 AngleToDirection(float vertical, float horizon)
    {
        Vector2 v2Direction = new Vector2(-Mathf.Sin(horizon), Mathf.Cos(horizon));
        Vector3 direction;
        direction.x = -(v2Direction.x * Mathf.Cos(vertical));
        direction.y = (Mathf.Sin(vertical));
        direction.z = -(v2Direction.y * Mathf.Cos(vertical));

        return direction.normalized;
    }

    public static void DirectionToAngle(Vector3 direction, ref float vertical, ref float horizon)
    {
        float xzMagnitute = Mathf.Sqrt(direction.z * direction.z + direction.x * direction.x);
        if (direction.x != 0)
        {
            horizon = Mathf.Atan2(direction.x, -direction.z);
            horizon = CheckRadian(horizon);
        }
        else
        {
            horizon = 0;
        }

        if (xzMagnitute != 0)
        {
            vertical = Mathf.Atan2(direction.y, xzMagnitute);
            vertical = CheckRadian(vertical);
        }
        else
        {
            vertical = 0;
        }


    }

    public static float GetAngleWithoutY(Vector3 directionA, Vector3 directionB)
    {
        float dot = Vector3.Dot(directionA, directionB);
        if (Mathf.Abs(dot) > (1.0f - s_fEpsilon))
        {
            return (dot > 0) ? 0 : s_fPI;
        }

        float angle = Mathf.Acos(dot);
        if (float.IsNaN(angle))
        {
            return 0;
        }

        Vector3 cross = Vector3.Cross(directionA, directionB);
        if (cross.y >= 0)
            return angle;
        else
            return -angle;
    }

    public static Vector2 GetScreenSizeInWorldCoords(Camera gameCamera, float distance = 10f)
    {
        float width = 0f;
        float height = 0f;

        if (gameCamera.orthographic)
        {
            if (gameCamera.orthographicSize <= .001f)
                return new Vector2(0, 0);

            var p1 = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, gameCamera.nearClipPlane));
            var p2 = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, gameCamera.nearClipPlane));
            var p3 = gameCamera.ViewportToWorldPoint(new Vector3(1, 1, gameCamera.nearClipPlane));

            width = (p2 - p1).magnitude;
            height = (p3 - p2).magnitude;
        }
        else
        {
            height = 2.0f * Mathf.Abs(distance) * Mathf.Tan(gameCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            width = height * gameCamera.aspect;
        }

        return new Vector2(width, height);
    }

    public enum eEaseType
    {
        EaseInOut,
        EaseOut,
        EaseIn,
        Linear
    }

    public static float EaseFromTo(float start, float end, float value, eEaseType type = eEaseType.EaseInOut)
    {
        switch (type)
        {
            case eEaseType.EaseInOut:
                return Mathf.Lerp(start, end, value * value * (3.0f - 2.0f * value));

            case eEaseType.EaseOut:
                return Mathf.Lerp(start, end, Mathf.Sin(value * Mathf.PI * 0.5f));

            case eEaseType.EaseIn:
                return Mathf.Lerp(start, end, 1.0f - Mathf.Cos(value * Mathf.PI * 0.5f));

            default:
                return Mathf.Lerp(start, end, value);
        }
    }


    public static int RandInt(int value)
    {
        return UnityEngine.Random.Range(0, value);
    }

    public static int RandRange(int rMin, int rMax)
    {
        return UnityEngine.Random.Range(rMin, rMax);
    }

    public static float RandRange(float rMin, float rMax)
    {
        return UnityEngine.Random.Range(rMin, rMax);
    }

    public static Vector3 RandomDirection()
    {
        float sinPitch = RandRange(-1.0f, 1.0f);
        float cosPitch = Mathf.Sqrt(1.0f - sinPitch * sinPitch);
        float yaw = RandRange(0.0f, s_f2PI);

        return new Vector3(Mathf.Cos(yaw) * cosPitch, Mathf.Sin(yaw) * cosPitch, sinPitch);
    }

    public static float SmoothApproach(float pastPosition, float pastTargetPosition, float targetPosition, float speed, float deltaTime)
    {
        float t = deltaTime * speed;
        float v = (targetPosition - pastTargetPosition) / t;
        float f = pastPosition - pastTargetPosition + v;
        return targetPosition - v + f * Mathf.Exp(-t);
    }

    public static float Pitch(Quaternion quaternion)
    {
        float test = quaternion.x * quaternion.y + quaternion.z * quaternion.w;
        if (Mathf.Abs(test) > 0.499f) // singularity at north and south pole
            return 0;
        return Mathf.Atan2(2.0f * quaternion.x * quaternion.w - 2.0f * quaternion.y * quaternion.z,
            1.0f - 2.0f * quaternion.x * quaternion.x - 2.0f * quaternion.z * quaternion.z);
    }

    public static float Yaw(Quaternion quaternion)
    {
        float test = quaternion.x * quaternion.y + quaternion.z * quaternion.w;
        if (Mathf.Abs(test) > 0.499f) // singularity at north and south pole
            return Mathf.Sin(test) * 2.0f * Mathf.Atan2(quaternion.x, quaternion.w);
        return Mathf.Atan2(2.0f * quaternion.y * quaternion.w - 2.0f * quaternion.x * quaternion.z,
            1.0f - 2.0f * quaternion.y * quaternion.y - 2.0f * quaternion.z * quaternion.z);
    }

    public static float Roll(Quaternion quaternion)
    {
        float test = quaternion.x * quaternion.y + quaternion.z * quaternion.w;
        if (Mathf.Abs(test) > 0.499f) // singularity at north and south pole
            return Mathf.Sin(test) * s_fHalfPI;
        return Mathf.Asin(2.0f * test);
    }

    public static Vector3 ToEulerAngles(Quaternion quaternion)
    {
        float pitch, yaw, roll;
        ToEulerAngles(quaternion, out yaw, out pitch, out roll);
        return new Vector3(yaw, pitch, roll);
    }

    public static void ToEulerAngles(Quaternion quaternion, out float yaw, out float pitch, out float roll)
    {
        float test = quaternion.x * quaternion.y + quaternion.z * quaternion.w;
        if (test > 0.499f)
        { // singularity at north pole
            yaw = 2.0f * Mathf.Atan2(quaternion.x, quaternion.w);
            roll = s_fHalfPI;
            pitch = 0;
        }
        else if (test < -0.499f)
        { // singularity at south pole
            yaw = -2.0f * Mathf.Atan2(quaternion.x, quaternion.w);
            roll = -s_fHalfPI;
            pitch = 0;
        }
        else
        {
            float sqx = quaternion.x * quaternion.x;
            float sqy = quaternion.y * quaternion.y;
            float sqz = quaternion.z * quaternion.z;
            yaw = Mathf.Atan2(2.0f * quaternion.y * quaternion.w - 2.0f * quaternion.x * quaternion.z,
                1.0f - 2.0f * sqy - 2.0f * sqz);
            roll = Mathf.Asin(2 * test);
            pitch = Mathf.Atan2(2 * quaternion.x * quaternion.w - 2.0f * quaternion.y * quaternion.z,
                1.0f - 2.0f * sqx - 2.0f * sqz);
        }

        if (pitch < 0) pitch += s_f2PI;
        if (yaw < 0) yaw += s_f2PI;
        if (roll < 0) roll += s_f2PI;
    }

    public static Quaternion GetDirectionToQuaternion(Vector3 v1)
    {
        Quaternion quaternion = Quaternion.identity;

        Vector3 v0 = new Vector3(0, 0, 1);
        Vector3 c = Vector3.Cross(v0, v1);

        // If the cross product approaches zero, we get unstable because ANY axis will do
        // when v0 == -v1
        float dist = Vector3.Dot(v0, v1);

        if (dist >= 1.0f)
        {
            return quaternion;
        }
        if (dist <= -1.0f)
        {
            quaternion = new Quaternion(0, 1, 0, 0);
            return quaternion;
        }
        //if (dist <= -1.0f) return new Quaternion(0, 0, 0, -1);

        float s = Mathf.Sqrt((1 + dist) * 2);
        float inverse = 1 / s;

        quaternion.x = c.x * inverse;
        quaternion.y = c.y * inverse;
        quaternion.z = c.z * inverse;
        quaternion.w = s * 0.5f;

        return quaternion;
    }

    /// <summary>
    /// Limit the magnitude of a vector to the specified max
    /// </summary>
    /// <param type="Vector3" name="v"></param>
    /// <param type="float" name="max"></param>
    public static Vector3 Limit(Vector3 v, float max)
    {
        if (v.magnitude > max)
        {
            return v.normalized * max;
        }
        return v;
    }

    public static int GetHashKeyFilePath(string path)
    {
        path = path.Replace("\\", "/");
        string[] split = path.Split('/');
        string fileName = split[split.Length - 1];

        return LEMath.GenerateHashKey(fileName);
    }

    public static int GenerateHashKey(string s)
    {
        int hash = 0;
        int len = s.Length;

        unchecked
        {
            uint poly = 0xEDB88320;
            for (int i = 0; i < len; i++)
            {
                poly = (poly << 1) | (poly >> (32 - 1)); // 1bit Left Shift
                hash = (int)(poly * hash + s[i]);
            }
        }

        return hash;
    }

    public static Vector3 Bezier3(Vector3 p1, Vector3 p2, Vector3 p3, float mu)
    {

        float mum1, mum12, mu2;

        Vector3 p = Vector3.zero;
        mu2 = mu * mu;
        mum1 = 1 - mu;
        mum12 = mum1 * mum1;
        p.x = p1.x * mum12 + 2 * p2.x * mum1 * mu + p3.x * mu2;
        p.y = p1.y * mum12 + 2 * p2.y * mum1 * mu + p3.y * mu2;
        p.z = p1.z * mum12 + 2 * p2.z * mum1 * mu + p3.z * mu2;

        return (p);
    }

    public static bool OperationCheck(E_MathOperation eOperation, int a, int b)
    {
        switch (eOperation)
        {
            case E_MathOperation.LessThan:
                return a < b ? true : false;
            case E_MathOperation.LessThanOrEqualTo:
                return a <= b ? true : false;
            case E_MathOperation.EqualTo:
                return a == b ? true : false;
            case E_MathOperation.NotEqualTo:
                return a != b ? true : false;
            case E_MathOperation.GreaterThanOrEqualTo:
                return a >= b ? true : false;
            case E_MathOperation.GreaterThan:
                return a > b ? true : false;
        }
        return false;
    }

    public static bool OperationCheck(E_MathOperation eOperation, float a, float b)
    {
        switch (eOperation)
        {
            case E_MathOperation.LessThan:
                return a < b ? true : false;
            case E_MathOperation.LessThanOrEqualTo:
                return a <= b ? true : false;
            case E_MathOperation.EqualTo:
                return a == b ? true : false;
            case E_MathOperation.NotEqualTo:
                return a != b ? true : false;
            case E_MathOperation.GreaterThanOrEqualTo:
                return a >= b ? true : false;
            case E_MathOperation.GreaterThan:
                return a > b ? true : false;
        }
        return false;
    }

    public static Vector3 Times(this Vector3 self, Vector3 other)
    {
        return new Vector3(self.x * other.x, self.y * other.y, self.z * other.z);
    }

    public static Quaternion ExtractRotation(this Matrix4x4 matrix)
    {
        Vector3 forward;
        forward.x = matrix.m02;
        forward.y = matrix.m12;
        forward.z = matrix.m22;

        Vector3 upwards;
        upwards.x = matrix.m01;
        upwards.y = matrix.m11;
        upwards.z = matrix.m21;

        return Quaternion.LookRotation(forward, upwards);
    }

    public static Vector3 ExtractPosition(this Matrix4x4 matrix)
    {
        Vector3 position;
        position.x = matrix.m03;
        position.y = matrix.m13;
        position.z = matrix.m23;
        return position;
    }

    public static Vector3 ExtractScale(this Matrix4x4 matrix)
    {
        Vector3 scale;
        scale.x = new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30).magnitude;
        scale.y = new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31).magnitude;
        scale.z = new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32).magnitude;
        return scale;
    }

    public static List<Vector3> GetCorners(this Bounds obj, bool includePosition = true)
    {
        var result = new List<Vector3>();
        for (int x = -1; x <= 1; x += 2)
        {
            for (int y = -1; y <= 1; y += 2)
            {
                for (int z = -1; z <= 1; z += 2)
                {
                    result.Add((includePosition ? obj.center : Vector3.zero) + (obj.size / 2).Times(new Vector3(x, y, z)));
                }
            }
        }

        return result;
    }

    public static void FromMatrix(this Transform transform, Matrix4x4 matrix)
    {
        transform.localScale = matrix.ExtractScale();
        transform.rotation = matrix.ExtractRotation();
        transform.position = matrix.ExtractPosition();
    }

    public static Bounds TransformBounds(this Transform self, Bounds localBounds)
    {
        var center = self.TransformPoint(localBounds.center);
        var points = localBounds.GetCorners();

        var result = new Bounds(center, Vector3.zero);
        foreach (var point in points)
            result.Encapsulate(self.TransformPoint(point));
        return result;
    }
    public static Bounds InverseTransformBounds(this Transform self, Bounds worldBounds)
    {
        var center = self.InverseTransformPoint(worldBounds.center);
        var points = worldBounds.GetCorners();

        var result = new Bounds(center, Vector3.zero);
        foreach (var point in points)
            result.Encapsulate(self.InverseTransformPoint(point));
        return result;
    }

    public const int INT_UNIT = 1000; // 0.01 -> 100; 0.001->1000
    public static int FloatToBigInt(float f)
    {
        return Mathf.RoundToInt(f * INT_UNIT);
    }
}
