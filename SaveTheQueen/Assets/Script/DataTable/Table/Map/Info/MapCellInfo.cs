namespace Aniz.Data.Map.Info
{
    public class MapCellInfo
    {
        public uint Index { get; set; }
        public uint Color { get; set; }

        public bool MoveBlocked { get; set; }
        public bool RemoteBlocked { get; set; }
        public string desc { get; set; }
    }
}