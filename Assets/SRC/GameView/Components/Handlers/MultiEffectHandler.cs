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
        [Inject] private readonly ClickHandler<IPlayable> _clickHandler;
        [Inject] private readonly ZoneViewsManager _zoneViewsManager;
        [Inject] private readonly MoveCardHandler _moveCardHandler;
        private CardView originalCardView;
        private List<IPlayable> cardViewClones;

        /*******************************************************************/
        public async Task<Effect> ShowMultiEffects(CardView cardViewWithMultiEffecs, string title)
        {
            await DotweenExtension.WaitForAnimationsComplete();
            if (cardViewWithMultiEffecs == null) throw new ArgumentNullException(nameof(cardViewWithMultiEffecs));
            originalCardView = cardViewWithMultiEffecs;
            cardViewClones = CreateCardViewClones();

            await _showSelectorComponent.ShowCards(cardViewClones.Cast<CardView>().ToList(), title);
            _showCardHandler.ActiavatePlayables(cardViewClones);
            IPlayable playableSelected = await _clickHandler.WaitingClick();
            return await FinishMultiEffect(playableSelected);
        }

        private async Task<Effect> FinishMultiEffect(IPlayable playableSelected)
        {
            await _showCardHandler.DeactivatePlayables(cardViewClones);

            if (playableSelected is CardView cardView)
            {
                Effect effectSelected = playableSelected.EffectsSelected.Single();
                originalCardView.ClearCloneEffect();
                (originalCardView.transform.position, cardView.transform.position) = (cardView.transform.position, originalCardView.transform.position);

                Sequence destroyClonesSequence = DOTween.Sequence();
                cardViewClones.Cast<CardView>().Except(new[] { originalCardView })
                    .ForEach(clone => destroyClonesSequence.Join(clone.MoveToZone(_zoneViewsManager.OutZone, Ease.InSine))
                        .OnComplete(() => GameObject.Destroy(clone.gameObject)));
                destroyClonesSequence.Join(_moveCardHandler.MoveCardViewWithPreviewToZone(originalCardView, _zoneViewsManager.Get(originalCardView.Card.CurrentZone)));

                await _showSelectorComponent.ShowDown(destroyClonesSequence, withActivation: false);
                cardViewClones = null;
                return effectSelected;
            }
            else
            {
                Sequence returnClonesSequence = DOTween.Sequence();
                cardViewClones.Cast<CardView>().Except(new[] { originalCardView })
                .ForEach(clone => returnClonesSequence.Join(clone.MoveToZone(_zoneViewsManager.CenterShowZone, Ease.InSine)
                    .OnComplete(() => GameObject.Destroy(clone.gameObject))));
                returnClonesSequence.Join(_moveCardHandler.MoveCardViewWithPreviewToZone(originalCardView, _zoneViewsManager.Get(originalCardView.Card.CurrentZone)));

                await _showSelectorComponent.ShowDown(returnClonesSequence, withActivation: false);
                originalCardView.ClearCloneEffect();
                cardViewClones = null;
                return null;
            }
        }

        private List<IPlayable> CreateCardViewClones()
        {
            List<Effect> effects = originalCardView.Card.PlayableEffects.ToList();
            originalCardView.SetCloneEffect(effects.First());
            List<IPlayable> newClonesCardView = new() { originalCardView };
            foreach (Effect effect in effects.Skip(1))
            {
                CardView cloneCardView = originalCardView.CloneToMultiEffect(originalCardView.CurrentZoneView.transform);
                cloneCardView.SetCloneEffect(effect);
                newClonesCardView.Add(cloneCardView);
            }
            return newClonesCardView;
        }
    }
}
