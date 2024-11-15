﻿using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class ZoneOutView : ZoneView
    {
        [SerializeField, Required, ChildGameObjectsOnly] protected Transform _movePosition;

        /*******************************************************************/
        public override Tween EnterZone(CardView cardView) =>
            cardView.transform.DOFullLocalMove(_movePosition, ViewValues.DEFAULT_TIME_ANIMATION).OnComplete(() => cardView.Off());

        public override Tween ExitZone(CardView cardView) => DOTween.Sequence().OnStart(() => cardView.On());

        public override Tween MouseEnter(CardView cardView) => DOTween.Sequence();

        public override Tween MouseExit(CardView cardView) => DOTween.Sequence();
    }
}
