using DG.Tweening;
using MythosAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class MoveCardHandler
    {
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly ZoneViewsManager _zonesViewManager;
        [Inject] private readonly SwapInvestigatorHandler _swapInvestigatorHandler;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] protected readonly AudioComponent _audioComponent;

        /*******************************************************************/
        public Tween MoveCardWithPreviewToZone(Card card, Zone zone)
        {
            CardView cardView = _cardViewsManager.GetCardView(card);
            ZoneView zoneView = _zonesViewManager.Get(zone);

            return MoveCardViewWithPreviewToZone(cardView, zoneView);
        }

        private Tween MoveCardViewWithPreviewToZone(CardView cardView, ZoneView zoneView)
        {
            return DOTween.Sequence()
             .Append(MoveCardViewToCenter(cardView))
             .Append(_swapInvestigatorHandler.Select(zoneView.Zone))
             .Append(cardView.MoveToZone(zoneView, Ease.InSine).OnPlay(() => _audioComponent.PlayMoveCardAudio()));
        }

        public Tween MoveCardWithPreviewWithoutWait(Card card, Zone zone)
        {
            CardView cardView = _cardViewsManager.GetCardView(card);
            ZoneView zoneView = _zonesViewManager.Get(zone);
            return DOTween.Sequence()
                .Append(MoveCardViewToCenter(cardView))
                .Append(_swapInvestigatorHandler.Select(zoneView.Zone))
                .OnComplete(() => cardView.MoveToZone(zoneView, Ease.InSine).OnPlay(() => _audioComponent.PlayMoveCardAudio()));
        }

        public Tween MoveCardtoCenter(Card card)
        {
            CardView cardView = _cardViewsManager.GetCardView(card);
            return MoveCardViewToCenter(cardView);
        }

        public Tween MoveCardViewToCenter(CardView cardView)
        {
            if (cardView.CurrentZoneView == _zonesViewManager.CenterShowZone) return DOTween.Sequence();

            return DOTween.Sequence()
                .Append(_swapInvestigatorHandler.Select(cardView.CurrentZoneView.Zone))
                .Append(cardView.MoveToZone(_zonesViewManager.CenterShowZone, Ease.OutSine).OnPlay(() => _audioComponent.PlayMoveCardCenterAudio()));
        }

        public Tween ReturnCard(Card card) => MoveCardWithPreviewToZone(card, card.CurrentZone);

        public Tween MoveCardsToCurrentZones(IEnumerable<Card> cards, float delay = 0f)
        {
            Dictionary<CardView, ZoneView> cardViewsWithZones = cards.ToDictionary(card => _cardViewsManager.GetCardView(card), card => _zonesViewManager.Get(card.CurrentZone));
            return MoveCardViewsToZones(cardViewsWithZones, delay);
        }

        public Tween MoveCardViewsToZones(Dictionary<CardView, ZoneView> cardViewsWithZones, float delay)
        {
            float delayBetweenMoves = 0f;
            Sequence sequence = DOTween.Sequence()
            .OnPlay(() => _audioComponent.PlayMoveDeckAudio(delay > 0))
            .OnComplete(() => _audioComponent.StopAudio());
            cardViewsWithZones.ForEach(cardView => sequence.Insert(delayBetweenMoves += delay, cardView.Key.MoveToZone(cardView.Value, Ease.InSine)));

            Investigator owner = cardViewsWithZones.Select(cardView => _investigatorsProvider.GetInvestigatorWithThisZone(cardView.Value.Zone)).UniqueOrDefault() ??
                cardViewsWithZones.Select(cardView => cardView.Key.Card.Owner).UniqueOrDefault();

            return DOTween.Sequence().Append(_swapInvestigatorHandler.Select(owner)).Append(sequence);
        }
    }
}
