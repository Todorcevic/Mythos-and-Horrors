using System;

namespace MythsAndHorrors.GameRules
{
    [Flags]
    public enum Faction
    {
        None = 0,
        Versatile = 1 << 0,
        Intrepid = 1 << 1,
        Valiant = 1 << 2,
        Esoteric = 1 << 3,
        Scholarly = 1 << 4,
        Neutral = 1 << 5,
        Myths = 1 << 6,
    }
}
