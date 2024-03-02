using System.Collections.Generic;

namespace MythosAndHorrors.GameRules
{
    public interface IEffectable
    {
        List<Effect> PlayableEffects { get; }
        bool CanBePlayed => PlayableEffects.Count > 0;
    }
}
