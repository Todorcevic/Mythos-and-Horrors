using DG.Tweening;
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
        [Inject] private readonly ClickHandler _clickHandler;
        [Inject] private readonly MoveCardHandler _moveCardHandler;
        private CardView originalCardView;
        private List<IPlayable> cardViewClones;

        /*******************************************************************/
        public async Task<BaseEffect> ShowMultiEffects(CardView cardViewWithMultiEffecs, string title)
        {
            if (cardViewWithMultiEffecs == null) throw new ArgumentNullException(nameof(cardViewWithMultiEffecs));
            originalCardView = cardViewWithMultiEffecs;
            await _moveCardHandler.MoveCardViewToCenter(originalCardView).AsyncWaitForCompletion();
            cardViewClones = CreateCardViewClones();
            originalCardView.gameObject.SetActive(false);
            await _showSelectorComponent.ShowCards(cardViewClones.Cast<CardView>().ToList(), title);

            Task<IPlayable> waitClick = _clickHandler.WaitingClick();
            _showCardHandler.ActivatePlayables(cardViewClones);
            IPlayable playableSelected = await waitClick;

            await FinishMultiEffect();
            return playableSelected.EffectsSelected.Single();
        }

        private async Task FinishMultiEffect()
        {
            await _showCardHandler.DeactivatePlayables(cardViewClones);
            Sequence destroyClonesSequence = DOTween.Sequence();
            cardViewClones.Cast<CardView>()
                .ForEach(clone => destroyClonesSequence.Join(_moveCardHandler.MoveCardViewToCenter(clone)
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
