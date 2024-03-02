using System;

namespace MythsAndHorrors.GameRules
{
    [Flags]
    public enum SlotType
    {
        None = 0,
        Trinket = 1 << 0,
        Equipment = 1 << 1,
        Supporter = 1 << 2,
        Item = 1 << 3,
        Magical = 1 << 4,
    }
}
