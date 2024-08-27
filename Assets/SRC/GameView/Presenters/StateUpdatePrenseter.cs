using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class StateUpdatePrenseter : IPresenter<UpdateStatesGameAction>
    {
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly SwapInvestigatorHandler _swapInvestigatorPresenter;

        /*******************************************************************/
        async Task IPresenter<UpdateStatesGameAction>.PlayAnimationWith(UpdateStatesGameAction updateStatesGameAction)
        {
            await CheckIfCardIsExhausted(updateStatesGameAction.States).AsyncWaitForCompletion();
            await CheckIfCardIsRevealed(updateStatesGameAction.States).AsyncWaitForCompletion();
            await CheckIfCardIsBlanked(updateStatesGameAction.States).AsyncWaitForCompletion();
        }

        private Tween CheckIfCardIsExhausted(IEnumerable<State> states)
        {
            IEnumerable<Card> cardsUpdated = _cardsProvider.AllCards.Where(card => states.Contains(card.Exausted));
            if (!cardsUpdated.Any()) return DOTween.Sequence();
            Sequence readySequence = DOTween.Sequence()
                .Append(_swapInvestigatorPresenter.Select(cardsUpdated.Select(card => card.Owner).UniqueOrDefault()));
            readySequence.Append(DOTween.Sequence());
            foreach (Card card in cardsUpdated)
            {
                CardView cardView = _cardViewsManager.GetCardView(card);
                readySequence.Join(cardView.CheckExhaust());
            }
            return readySequence;
        }

        private Tween CheckIfCardIsRevealed(IEnumerable<State> states)
        {
            IEnumerable<Card> cardsUpdated = _cardsProvider.AllCards.OfType<IRevealable>().Where(revelable => states.Contains(revelable.Revealed)).FilterCast<Card>();
            if (!cardsUpdated.Any()) return DOTween.Sequence();
            Sequence readySequence = DOTween.Sequence()
                .Append(_swapInvestigatorPresenter.Select(cardsUpdated.Select(revelable => revelable.Owner).UniqueOrDefault()));
            readySequence.Append(DOTween.Sequence());
            foreach (Card card in cardsUpdated)
            {
                CardView cardView = _cardViewsManager.GetCardView(card);
                readySequence.Join(cardView.RevealAnimation());
            }
            return readySequence;
        }

        private Tween CheckIfCardIsBlanked(IEnumerable<State> states)
        {
            IEnumerable<Card> cardsUpdated = _cardsProvider.AllCards.Where(card => states.Contains(card.Blancked));
            if (!cardsUpdated.Any()) return DOTween.Sequence();
            Sequence readySequence = DOTween.Sequence()
                .Append(_swapInvestigatorPresenter.Select(cardsUpdated.Select(card => card.Owner).UniqueOrDefault()));
            readySequence.Append(DOTween.Sequence());
            foreach (Card card in cardsUpdated)
            {
                CardView cardView = _cardViewsManager.GetCardView(card);
                readySequence.Join(cardView.CheckBlancked());
            }
            return readySequence;
        }
    }
}
