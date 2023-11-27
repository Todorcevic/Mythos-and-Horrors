using System;

namespace MythsAndHorrors.GameRules
{
    [Flags]
    public enum ZoneType
    {
        None = 0,
        DangerDeck = 1 << 0,
        DangerDiscard = 1 << 1,
        Goal = 1 << 2,
        Plot = 1 << 3,
        Victory = 1 << 4,
        Limbo = 1 << 5,
        Out = 1 << 6,
        Selector = 1 << 7,
        Place = 1 << 8,
        Hand = 1 << 9,
        Aid = 1 << 10,
        Danger = 1 << 11,
        Adventurer = 1 << 12,
        AdventurerDiscard = 1 << 13,
        AdventurerDeck = 1 << 14,
    }
}
