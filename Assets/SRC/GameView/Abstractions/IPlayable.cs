using MythsAndHorrors.GameRules;
using System.Collections.Generic;

namespace MythsAndHorrors.GameView
{
    public interface IPlayable
    {
        List<Effect> EffectsSelected { get; }
        bool IsMultiEffect => EffectsSelected.Count > 1;
    }
}
