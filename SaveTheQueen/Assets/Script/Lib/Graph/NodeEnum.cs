
namespace Aniz
{
    public enum eNodeStorageType
    {
        Scene,
        Entity,
        Actor,
        Max,
    }

    public enum eNodeCategory
    {
        Node = 0x0100,
        Group = 0x0200,
        Level = 0x0300,
        Scene = 0x0400,

        Entity = 0x0700,
        Actor = 0x0800,
    }

    public enum eNodeType : int
    {
        None = 0,

        //NodeGroup = (int)eNodeCategory.Node << 16 | 0x0001,
        State = (int)eNodeCategory.Node << 16 | 0x0002,
        PoolState = (int)eNodeCategory.Node << 16 | 0x0011,
        MonoNodeGroup = (int)eNodeCategory.Node << 16 | 0x0021,

        Spawner = (int)eNodeCategory.Node << 16 | 0x0031,
        HeroSpawner = (int)eNodeCategory.Node << 16 | 0x0032,

        Trigger = (int)eNodeCategory.Node << 16 | 0x0041,
        Camera = (int)eNodeCategory.Node << 16 | 0x0101,

        ActorRoot = (int)eNodeCategory.Node << 16 | 0x0100,

        ActorImpl = (int)eNodeCategory.Node << 16 | 0x1001,
        EntityImpl = (int)eNodeCategory.Node << 16 | 0x1101,
        TileImpl = (int)eNodeCategory.Node << 16 | 0x1111,

        WorldLevel = (int)eNodeCategory.Level << 16 | 0x0001,
        TileMapLevel = (int)eNodeCategory.Level << 16 | 0x0003,
        StageLevel = (int)eNodeCategory.Level << 16 | 0x0005,
        ActorLevel = (int)eNodeCategory.Level << 16 | 0x0007,

        Tile = eNodeCategory.Entity << 16 | 0x0001,
        Destroyable = (int)eNodeCategory.Entity << 16 | 0x0011,
        VFXEmitter = (int)eNodeCategory.Entity << 16 | 0x0030,
        SFXRoot = (int)eNodeCategory.Entity << 16 | 0x0101,
        SFXMixerNode = (int)eNodeCategory.Entity << 16 | 0x0111,
        BGMMixerNode = (int)eNodeCategory.Entity << 16 | 0x0112,


        Hero = (int)eNodeCategory.Actor << 16 | 0x1001,
        PC = (int)eNodeCategory.Actor << 16 | 0x1002,
        Monster = (int)eNodeCategory.Actor << 16 | 0x1003,
        Npc = (int)eNodeCategory.Actor << 16 | 0x1004,


        MAX,
    }
}