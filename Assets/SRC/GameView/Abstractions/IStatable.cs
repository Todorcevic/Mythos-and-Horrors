using DG.Tweening;
using MythosAndHorrors.GameRules;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public interface IStatable
    {
        Stat Stat { get; }
        Transform StatTransform { get; }
        Tween UpdateAnimation();
    }
}
