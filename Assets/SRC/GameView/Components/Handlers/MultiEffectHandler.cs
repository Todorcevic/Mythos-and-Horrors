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
            if (cardViewWithMultiEffecs == null) throw new ArgumentNullException(nameof(cardViewWithMultiEffecs));
            originalCardView = cardViewWithMultiEffecs;
            await originalCardView.MoveToZone(_zoneViewsManager.CenterShowZone, Ease.InSine).AsyncWaitForCompletion();
            cardViewClones = CreateCardViewClones();
            originalCardView.gameObject.SetActive(false);
            await _showSelectorComponent.ShowCards(cardViewClones.Cast<CardView>().ToList(), title);
            _showCardHandler.ActiavatePlayables(cardViewClones);
            IPlayable playableSelected = await _clickHandler.WaitingClick();
            return await FinishMultiEffect(playableSelected);
        }

        private async Task<Effect> FinishMultiEffect(IPlayable playableSelected)
        {
            Effect effectSelected = playableSelected is CardView cardView ? playableSelected.EffectsSelected.Single() : null;
            await _showCardHandler.DeactivatePlayables(cardViewClones);
            Sequence destroyClonesSequence = DOTween.Sequence();
            cardViewClones.Cast<CardView>()
                .ForEach(clone => destroyClonesSequence.Join(clone.MoveToZone(_zoneViewsManager.CenterShowZone, Ease.InSine).OnComplete(() => GameObject.Destroy(clone.gameObject))));
            destroyClonesSequence.Append(_moveCardHandler.MoveCardsToCurrentZones(new[] { originalCardView.Card })
                .OnStart(() => originalCardView.gameObject.SetActive(true)));
            await _showSelectorComponent.ShowDown(destroyClonesSequence, withActivation: false);
            cardViewClones = null;
            originalCardView = null;
            return effectSelected;
        }

        private List<IPlayable> CreateCardViewClones()
        {
            List<Effect> effects = originalCardView.Card.PlayableEffects.ToList();
            List<IPlayable> newClonesCardView = new();
            foreach (Effect effect in effects)
            {
                CardView cloneCardView = originalCardView.CloneToMultiEffect(originalCardView.CurrentZoneView.transform);
                cloneCardView.SetCloneEffect(effect);
                newClonesCardView.Add(cloneCardView);
            }
            return newClonesCardView;
        }
    }
}
