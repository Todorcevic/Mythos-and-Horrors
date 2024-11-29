using MythosAndHorrors.GameRules;
using System.Collections.Generic;

namespace MythosAndHorrors.GameView
{
    public interface IPlayable
    {
        IEnumerable<BaseEffect> EffectsSelected { get; }
        bool CanBePlayed { get; }
        void ActivateToClick();
        void DeactivateToClick();
    }
}
