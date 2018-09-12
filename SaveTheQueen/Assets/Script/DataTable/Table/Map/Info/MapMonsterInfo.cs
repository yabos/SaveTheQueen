using UnityEngine;

namespace Aniz.Data.Map.Info
{
    public class MapMonsterInfo
    {
        public uint ActorID { get; set; }
        public uint ZoneGroup { get; set; }
        public Vector3 Pos { get; set; }
        public byte Direction { get; set; }
    }
}