using UnityEngine;

namespace Aniz.Cam.Info
{
    public class CameraUpdateInfo
    {
        public float Distance { get; set; }
        public float Vertical { get; set; }
        public float Horizon { get; set; }
        public float Height { get; set; }

        public Vector3 Target { get; set; }
        public Vector3 Eye { get; set; }
        public Vector3 At { get; set; }
        public Vector3 Up { get; set; }
        public Vector3 LookatDir { get; set; }

        public CameraUpdateInfo()
        {
            Distance = 10;
            Vertical = 0;
            Horizon = 0;
            Height = 1.0f;
        }


        public Vector3 getOffset()
        {
            return LEMath.AngleToDirection(Vertical, Horizon) * Distance;
            // 		float radianangle = angle * Mathf.Deg2Rad;
            // 		offset.z = -Mathf.Cos( radianangle ) * length;
            // 		offset.y = Mathf.Sin( radianangle ) * length;
            // 		return offset;
        }

        public Vector2 getOffset2()
        {
            Vector2 offset2 = new Vector2();
            float radianangle = Vertical * Mathf.Deg2Rad;
            offset2.y = -Mathf.Cos(radianangle) * Distance;
            offset2.x = -Mathf.Sin(radianangle) * Distance;
            return offset2;
        }

        public Vector2 getProCameraOffsetY()
        {
            Vector2 offset2 = new Vector2();
            float radianangle = Vertical * Mathf.Deg2Rad;
            //offset2.x = -Mathf.Cos(radianangle) * length;
            offset2.y = Mathf.Sin(radianangle) * Distance + 2.0f;
            return offset2;
        }

    }
}