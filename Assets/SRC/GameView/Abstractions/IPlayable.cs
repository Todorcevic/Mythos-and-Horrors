using MythosAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;

namespace MythosAndHorrors.GameView
{
    public interface IPlayable
    {
        IEnumerable<Effect> EffectsSelected { get; }
        bool IsMultiEffect => EffectsSelected.Count() > 1;
        bool CanBePlayed => EffectsSelected.Count() > 0;
        void ActivateToClick();
        void DeactivateToClick();
    }
}
