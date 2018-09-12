using System;

namespace Lib.InputButton
{

    // do not change values
    public enum ePlayerButtonEnum
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3,

        JUMP = 4,
        FIRE1 = 5, // "fire1" / "block"
        FIRE2 = 6, // "fire2" / "lightAttack"
        FIRE3 = 7, // "fire3" / "heavyAttack"

        ABILITY1 = 8,
        ABILITY2 = 9,
        ABILITY3 = 10,
        ABILITY4 = 11,

        CHAIN1 = 12,
        CHAIN2 = 13,
        CHAIN3 = 14,
        CHAIN4 = 15,

        Item1 = 16,
        Item2 = 17,
        Item3 = 18,
        Item4 = 19,

        Revive1 = 20,
        QTE = 21,
        Revive3 = 22,
        Revive4 = 23,

        Powerup = 24,
        Pause = 25,
        Skip = 26,

        kMax
    }

    // do not change values
    [Flags]
    public enum ePlayerButton
    {
        None = 0x00000000,
        Up = 0x00000001,
        Down = 0x00000002,
        Left = 0x00000004,
        Right = 0x00000008,

        JUMP = 0x00000010,
        FIRE1 = 0x00000020,
        FIRE2 = 0x00000040,
        FIRE3 = 0x00000080,

        ABILITY1 = 0x00000100,
        ABILITY2 = 0x00000200,
        ABILITY3 = 0x00000400,
        ABILITY4 = 0x00000800,

        CHAIN1 = 0x00001000,
        CHAIN2 = 0x00002000,
        CHAIN3 = 0x00004000,
        CHAIN4 = 0x00008000,

        Item1 = 0x00010000,
        Item2 = 0x00020000,
        Item3 = 0x00040000,
        Item4 = 0x00080000,

        Revive1 = 0x00100000,
        QTE = 0x00200000,
        Revive3 = 0x00400000,
        Revive4 = 0x00800000,

        Powerup = 0x01000000,
        Pause = 0x02000000,
        Skip = 0x04000000,
    }

    public enum ePlayerButtonMasks
    {
        MovementControls = 0x0000000F,
        //	16방향 이동에 따라서 레인지 확장
        CharacterControls = 0x00FFFFFF,
        AllControls = 0x004FFFFF,
    }



}