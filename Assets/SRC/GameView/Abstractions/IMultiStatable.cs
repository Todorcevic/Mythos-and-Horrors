using MythosAndHorrors.GameRules;
using System.Collections.Generic;

namespace MythosAndHorrors.GameView
{
    public interface IMultiStatable : IStatable
    {
        List<Stat> MultiStat { get; }
    }
}
