using DG.Tweening;
using MythsAndHorrors.GameRules;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public interface IStatable
    {
        Stat Stat { get; }
        Transform StatTransform { get; }
        Tween UpdateValue(int value);
        Tween IncreaseValue(int value);
        Tween DecreaseValue(int value);
    }
}
