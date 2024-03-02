using System;

namespace MythosAndHorrors.GameRules
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
        Place = 1 << 7,
        Hand = 1 << 8,
        Aid = 1 << 9,
        Danger = 1 << 10,
        Investigator = 1 << 11,
        InvestigatorDiscard = 1 << 12,
        InvestigatorDeck = 1 << 13,
        Own = 1 << 14
    }
}
