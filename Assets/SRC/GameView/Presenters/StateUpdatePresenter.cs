using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class StateUpdatePresenter
    {
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly SwapInvestigatorHandler _swapInvestigatorPresenter;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;

        /*******************************************************************/
        public async Task PlayAnimationWith(UpdateStatesGameAction updateStatesGameAction)
        {
            await CheckIfCardIsExhausted(updateStatesGameAction.States).AsyncWaitForCompletion();
            await CheckIfCardIsRevealed(updateStatesGameAction.States).AsyncWaitForCompletion();
            await CheckIfCardIsBlanked(updateStatesGameAction.States).AsyncWaitForCompletion();
            await CheckIfInvestigatorIsIsolated(updateStatesGameAction.States).AsyncWaitForCompletion();
        }

        private Tween CheckIfCardIsExhausted(IEnumerable<State> states)
        {
            IEnumerable<Card> cardsUpdated = _cardsProvider.AllCards.Where(card => states.Contains(card.Exausted));
            if (!cardsUpdated.Any()) return DOTween.Sequence();
            Sequence readySequence = DOTween.Sequence().Join(_swapInvestigatorPresenter.Select(cardsUpdated.Select(card => card.Owner).UniqueOrDefault()))
                .Append(DOTween.Sequence());
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
            Sequence readySequence = DOTween.Sequence().Join(_swapInvestigatorPresenter.Select(cardsUpdated.Select(revelable => revelable.Owner).UniqueOrDefault()))
                .Append(DOTween.Sequence());
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
            Sequence readySequence = DOTween.Sequence().Join(_swapInvestigatorPresenter.Select(cardsUpdated.Select(card => card.Owner).UniqueOrDefault()))
                .Append(DOTween.Sequence());
            foreach (Card card in cardsUpdated)
            {
                readySequence.Join(_cardViewsManager.GetCardView(card).CheckBlancked());
            }
            return readySequence;
        }

        private Tween CheckIfInvestigatorIsIsolated(IEnumerable<State> states)
        {
            Investigator investigator = _investigatorsProvider.AllInvestigatorsInPlay.FirstOrDefault(investigator => states.Contains(investigator.Isolated));
            if (investigator == null) return DOTween.Sequence();
            Sequence isolateInvestigatorSequence = DOTween.Sequence().Join(_swapInvestigatorPresenter.Select(investigator)).Append(DOTween.Sequence());
            if (investigator.Isolated.IsActive)
                _avatarViewsManager.AllAvatars.ForEach(avatarView => isolateInvestigatorSequence.Join(avatarView.Investigator.Isolated.IsActive ? avatarView.Show() : avatarView.Hide()));
            else
                _avatarViewsManager.AllAvatars.ForEach(avatarView => isolateInvestigatorSequence.Join(avatarView.Show()));
            return isolateInvestigatorSequence;
        }
    }
}
