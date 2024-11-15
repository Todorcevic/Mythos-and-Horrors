﻿using DG.Tweening;
using MythosAndHorrors.GameRules;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class PlotCounterController : CounterController, IStatable
    {
        private CardPlot _cardPlot;

        public Stat Stat { get; private set; }
        public Transform StatTransform => transform;

        /*******************************************************************/
        public void Init(CardPlot cardPlot)
        {
            _cardPlot = cardPlot;
            Stat = _cardPlot.Eldritch;

            EnableThisAmount(_cardPlot.Eldritch.Value);
            ShowThisAmount(_cardPlot.AmountOfEldritch);
            gameObject.SetActive(true);
        }

        public Tween UpdateAnimation()
        {
            ShowThisAmount(_cardPlot.AmountOfEldritch);
            return DOTween.Sequence();
        }
    }
}
