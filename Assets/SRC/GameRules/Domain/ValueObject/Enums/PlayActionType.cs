using System;

namespace MythosAndHorrors.GameRules
{
    [Flags]
    public enum PlayActionType
    {
        None = 0,
        Attack = 1 << 0,
        Elude = 1 << 1,
        Parley = 1 << 2,
        Resign = 1 << 3,
        Choose = 1 << 4,
        Investigate = 1 << 5,
        Move = 1 << 6,
        Activate = 1 << 7,
        Draw = 1 << 8,
        TakeResource = 1 << 9,
        Confront = 1 << 10,
        PlayFromHand = 1 << 11,
        Commit = 1 << 12,
        WithoutOpportunityAttack = Attack | Elude | Parley | Resign | Commit | Choose
    }
}
