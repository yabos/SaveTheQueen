using UnityEngine;

namespace Aniz.Data.Map.Info
{
    public class MapDataInfo
    {
        public enum E_TERRAINDIR 
        {
	        TOP = 0,
	        RIGHT = 1,
	        BOTTOM = 2,
	        LEFT = 3,
	        __MAX__,
        }

        public ushort Index { get; set; }
        public uint CameraIndex { get; set; }
        public uint DayTime { get; set; }
        
        public bool PreLoad { get; set; }
        public bool TimeEnable { get; set; }
        public Vector2 startPos { get; set; }
        public int MapSize { get; set; }

        public string name { get; set; }
        public string mapfile { get; set; }
        public string lightName { get; set; }
        public string bgmFile { get; set; }
        public string npcFile { get; set; }
        public string triggerFile { get; set; }
        
    }
}