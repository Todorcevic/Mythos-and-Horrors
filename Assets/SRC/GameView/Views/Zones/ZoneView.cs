﻿using DG.Tweening;
using MythosAndHorrors.GameRules;
using System.Linq;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public abstract class ZoneView : MonoBehaviour
    {
        [SerializeField] private bool _avoidCardShower;
        [Inject] private readonly ZoneViewsManager _zoneViewsManager;

        public Zone Zone { get; private set; }
        public bool AvoidCardShower => _avoidCardShower;
        public bool IsEmpty => Zone.Cards.Count() == 0;

        /*******************************************************************/
        public void Init(Zone zone = null)
        {
            Zone = zone;
            _zoneViewsManager.Add(this);
        }

        /*******************************************************************/
        public abstract Tween EnterZone(CardView cardView);

        public abstract Tween ExitZone(CardView cardView);

        public abstract Tween MouseEnter(CardView cardView);

        public abstract Tween MouseExit(CardView cardView);

        public virtual Tween Shuffle() => DOTween.Sequence();

        public virtual Tween UpdatePosition(Card card) => DOTween.Sequence();
    }
}
