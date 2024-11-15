﻿using DG.Tweening;
using MythosAndHorrors.GameRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class MultiEffectHandler
    {
        [Inject] private readonly BasicShowSelectorComponent _showSelectorComponent;
        [Inject] private readonly ActivatePlayablesHandler _showCardHandler;
        [Inject] private readonly ClickHandler<IPlayable> _clickHandler;
        [Inject] private readonly ZoneViewsManager _zoneViewsManager;
        [Inject] private readonly MoveCardHandler _moveCardHandler;
        private CardView originalCardView;
        private List<IPlayable> cardViewClones;

        /*******************************************************************/
        public async Task<BaseEffect> ShowMultiEffects(CardView cardViewWithMultiEffecs, string title)
        {
            if (cardViewWithMultiEffecs == null) throw new ArgumentNullException(nameof(cardViewWithMultiEffecs));
            originalCardView = cardViewWithMultiEffecs;
            await originalCardView.MoveToZone(_zoneViewsManager.CenterShowZone, Ease.InSine).AsyncWaitForCompletion();
            cardViewClones = CreateCardViewClones();
            originalCardView.gameObject.SetActive(false);
            await _showSelectorComponent.ShowCards(cardViewClones.Cast<CardView>().ToList(), title);
            _showCardHandler.ActiavatePlayables(cardViewClones);
            IPlayable playableSelected = await _clickHandler.WaitingClick();
            await FinishMultiEffect();
            return playableSelected.EffectsSelected.Single();
        }

        private async Task FinishMultiEffect()
        {
            await _showCardHandler.DeactivatePlayables(cardViewClones);
            Sequence destroyClonesSequence = DOTween.Sequence();
            cardViewClones.Cast<CardView>()
                .ForEach(clone => destroyClonesSequence.Join(clone.MoveToZone(_zoneViewsManager.CenterShowZone, Ease.OutSine)
                .OnComplete(() => GameObject.Destroy(clone.gameObject))));
            destroyClonesSequence.Append(_moveCardHandler.MoveCardsToCurrentZones(new[] { originalCardView.Card })
                .OnStart(() => originalCardView.gameObject.SetActive(true)));
            await _showSelectorComponent.ShowDown(destroyClonesSequence, withActivation: false);
            cardViewClones = null;
            originalCardView = null;
        }

        private List<IPlayable> CreateCardViewClones()
        {
            List<CardEffect> effects = originalCardView.Card.PlayableEffects.ToList();
            List<IPlayable> newClonesCardView = new();
            foreach (CardEffect effect in effects)
            {
                CardView cloneCardView = originalCardView.CloneToMultiEffect(originalCardView.CurrentZoneView.transform);
                cloneCardView.SetCloneEffect(effect);
                newClonesCardView.Add(cloneCardView);
            }
            return newClonesCardView;
        }
    }
}
