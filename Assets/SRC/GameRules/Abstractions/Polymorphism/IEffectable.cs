using System.Collections.Generic;

namespace MythsAndHorrors.GameRules
{
    public interface IEffectable
    {
        List<Effect> PlayableEffects { get; }
        bool CanBePlayed => PlayableEffects.Count > 0;
    }
}
