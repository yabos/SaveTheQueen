namespace Aniz.Event
{
    public enum eMessageCategory : int
    {
        System = 0x0100,
        Scene = 0x0200,
        Game = 0x0300,
        Actor = 0x0400,
        Player = 0x0500,

        Trigger = 0x0600,
        Spawner = 0x0601,

        Event = 0x0700,
        Network = 0x0800,
        UI = 0x0900,

        AI = 0x0a00,

        MouseCategoryMsg = 0x0001,
        KeyCategoryMsg = 0x0002,
    }

    public enum eMessage : int
    {
        None = 0,

        Asset = eMessageCategory.System << 16 | 0x0001,
        UI = eMessageCategory.System << 16 | 0x0002,
        Page = eMessageCategory.System << 16 | 0x0003,
        Plugin = eMessageCategory.System << 16 | 0x0004,
        Network = eMessageCategory.System << 16 | 0x0005,

        GameStartCameraPath = eMessageCategory.Scene << 16 | 0x0001,
        GameLobbyPC = eMessageCategory.Scene << 16 | 0x0011,
        LobbyNPCChangeAnimation = eMessageCategory.Scene << 16 | 0x0021,

        PageTransition = eMessageCategory.Scene << 16 | 0x0101,

        GameStartRoomInfo = eMessageCategory.Game << 16 | 0x0003,
        ActorDestory = eMessageCategory.Actor << 16 | 0x0001,
        //ui 제어 skill chain
        Pawn_ActiveStateChange = eMessageCategory.Actor << 16 | 0x5001,
        PC_SkillAction = eMessageCategory.Actor << 16 | 0x5002,

        MassiveDefenceGameStart = eMessageCategory.Game << 16 | 0x0101,
        AntarasGameStart = eMessageCategory.Game << 16 | 0x0102,

        CameraPathEvent = eMessageCategory.Event << 16 | 0x0001,
        ChangeCameraStock = eMessageCategory.Event << 16 | 0x0002,
        ChangeFBXPathRail = eMessageCategory.Event << 16 | 0x0003,

        DamageText = eMessageCategory.UI << 16 | 0x0001,
        InputChange = eMessageCategory.UI << 16 | 0x1001,
        ShowWnd = eMessageCategory.UI << 16 | 0x2001,
        AllCurrentShowHide = eMessageCategory.UI << 16 | 0x3001,

        //skillchain
        AddSkillChainSlot = eMessageCategory.UI << 16 | 0x5001,
        FocusSkillSlot = eMessageCategory.UI << 16 | 0x5002,
        RemoveSkillChainSlot = eMessageCategory.UI << 16 | 0x5003,
        EmptyFocusSkillSlot = eMessageCategory.UI << 16 | 0x5004,
        RemoveSelectSkillChainSlot = eMessageCategory.UI << 16 | 0x5005,
        SelectSkillChainSlotOption = eMessageCategory.UI << 16 | 0x5006,

        PawnSkillEnter = eMessageCategory.UI << 16 | 0x9001,
        PawnSkillExit = eMessageCategory.UI << 16 | 0x9002,

        BT_AI = eMessageCategory.AI << 16 | 0x0001,
        StateSpawnEnd = eMessageCategory.AI << 16 | 0x0002,

        TargetToSpawnGroupAlram = eMessageCategory.AI << 16 | 0x0011,

        //action
        StateCommand = eMessageCategory.Player << 16 | 0x2001,
        Action = eMessageCategory.Player << 16 | 0x2002,

        //Defeat = eMessageCategory.Player << 16 | 0x2006,
        Dead = eMessageCategory.Player << 16 | 0x2007,

        Hit = eMessageCategory.Network << 16 | 0x2005,
        Effect = eMessageCategory.Network << 16 | 0x2008,

        //spawner
        SpawnMonster = eMessageCategory.Spawner << 16 | 0x0001,
        DeadMonster = eMessageCategory.Spawner << 16 | 0x0001,

        //network
        HeroEnter = eMessageCategory.Network << 16 | 0x1001,
        PCEnter = eMessageCategory.Network << 16 | 0x1002,
        EnterMap = eMessageCategory.Network << 16 | 0x1003,
        MonsterEnter = eMessageCategory.Network << 16 | 0x1004,
        RemoteEnter = eMessageCategory.Network << 16 | 0x1005,

        Leave = eMessageCategory.Network << 16 | 0x1010,
        NotifyEffect = eMessageCategory.Network << 16 | 0x1020,
    }

    public enum eNetworkSubMessage : int
    {
        Connected,
        Disconnected,

        Login,
        Logout,

        JoinRoom,
        LeaveRoom,

        ChatMessage,
    }

}