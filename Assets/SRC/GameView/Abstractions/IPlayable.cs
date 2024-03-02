using MythosAndHorrors.GameRules;
using System.Collections.Generic;

namespace MythosAndHorrors.GameView
{
    public interface IPlayable
    {
        List<Effect> EffectsSelected { get; }
        bool IsMultiEffect => EffectsSelected.Count > 1;
        bool CanBePlayed => EffectsSelected.Count > 0;
        void ActivateToClick();
        void DeactivateToClick();
    }
}
