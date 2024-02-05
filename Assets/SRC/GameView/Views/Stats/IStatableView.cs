using DG.Tweening;
using MythsAndHorrors.GameRules;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public interface IStatableView
    {
        Stat Stat { get; }
        Transform StatTransform { get; }
        Tween UpdateValue();
    }
}
