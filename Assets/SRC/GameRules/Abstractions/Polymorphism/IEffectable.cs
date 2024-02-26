using System.Collections.Generic;

namespace MythsAndHorrors.GameRules
{
    public interface IEffectable
    {
        IReadOnlyList<Effect> PlayableEffects { get; }
    }
}
