using System;

namespace MythsAndHorrors.EditMode
{
    [Flags]
    public enum CardType
    {
        None = 0,
        Adventurer = 1 << 0,
        Supply = 1 << 1,
        Talent = 1 << 2,
        Condition = 1 << 3,
        Creature = 1 << 4,
        Adversity = 1 << 5,
        Place = 1 << 6,
        Goal = 1 << 7,
        Plot = 1 << 8
    }
}
