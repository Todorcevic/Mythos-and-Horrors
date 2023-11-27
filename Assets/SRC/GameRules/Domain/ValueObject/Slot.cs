using System;

namespace MythsAndHorrors.EditMode
{
    [Flags]
    public enum Slot
    {
        None = 0,
        Trinket = 1 << 0,
        Equipment = 1 << 1,
        Supporter = 1 << 2,
        Item = 1 << 3,
        Itemx2 = 1 << 4,
        Magical = 1 << 5,
        Magicalx2 = 1 << 6
    }
}
