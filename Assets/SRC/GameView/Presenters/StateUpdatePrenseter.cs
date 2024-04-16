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
            await CheckIfCardIsRevelaed(updateStatesGameAction.States).AsyncWaitForCompletion();
        }

        private Tween CheckIfCardIsExhausted(IEnumerable<State> states)
        {
            IEnumerable<Card> cardsUpdated = _cardsProvider.AllCards.Where(card => states.Contains(card.Exausted));
            Sequence readySequence = DOTween.Sequence()
                .Append(_swapInvestigatorPresenter.Select(cardsUpdated.Select(card => card.Owner).UniqueOrDefault()));
            readySequence.Append(DOTween.Sequence());
            foreach (Card card in cardsUpdated)
            {
                CardView cardView = _cardViewsManager.GetCardView(card);
                readySequence.Join(card.Exausted.IsActive ? cardView.Exhaust() : cardView.Ready());
            }
            return readySequence;
        }

        private Tween CheckIfCardIsRevelaed(IEnumerable<State> states)
        {
            IEnumerable<IRevealable> cardsUpdated = _cardsProvider.AllCards.OfType<IRevealable>().Where(revelable => states.Contains(revelable.Revealed));
            Sequence readySequence = DOTween.Sequence()
                .Append(_swapInvestigatorPresenter.Select(cardsUpdated.FilterCast<Card>().Select(revelable => revelable.Owner).UniqueOrDefault()));
            readySequence.Append(DOTween.Sequence());
            foreach (Card card in cardsUpdated.FilterCast<Card>())
            {
                CardView cardView = _cardViewsManager.GetCardView(card);
                readySequence.Join(cardView.RevealAnimation());
            }
            return readySequence;
        }
    }
}
