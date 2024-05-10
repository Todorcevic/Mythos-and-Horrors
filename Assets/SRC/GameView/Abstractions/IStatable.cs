using DG.Tweening;
using MythosAndHorrors.GameRules;
using System.Collections.Generic;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public interface IStatable
    {
        List<Stat> MultiStat => new();
        Stat Stat { get; }
        Transform StatTransform { get; }
        Tween UpdateValue();
    }
}
