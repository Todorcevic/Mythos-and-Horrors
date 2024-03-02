using DG.Tweening;
using MythosAndHorrors.GameRules;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public interface IStatableView
    {
        Stat Stat { get; }
        Transform StatTransform { get; }
        Tween UpdateValue();
    }
}
