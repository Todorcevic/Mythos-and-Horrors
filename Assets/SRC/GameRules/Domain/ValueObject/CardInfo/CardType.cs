using System;

namespace MythosAndHorrors.GameRules
{
    [Flags]
    public enum CardType
    {
        None = 0,
        Investigator = 1 << 0,
        Supply = 1 << 1,
        Talent = 1 << 2,
        Condition = 1 << 3,
        Creature = 1 << 4,
        Adversity = 1 << 5, //32
        Place = 1 << 6, //64
        Goal = 1 << 7, //128
        Plot = 1 << 8, //256
        Scene = 1 << 9, //512
        Obstacle = 1 << 10 //1024
    }
}
