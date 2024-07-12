using System;

namespace MythosAndHorrors.GameRules
{
    [Flags]
    public enum Faction
    {
        None = 0,
        Versatile = 1 << 0,
        Cunning = 1 << 1,
        Brave = 1 << 2,
        Esoteric = 1 << 3,
        Scholarly = 1 << 4,
        Neutral = 1 << 5,
        Myth = 1 << 6,
    }
}
