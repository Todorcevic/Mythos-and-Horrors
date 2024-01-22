using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class PresenterManager : IViewLayer
    {
        [Inject] private readonly List<IPresenter> _allPresenters;

        /*******************************************************************/
        public async Task PlayAnimationWith(GameAction gameAction)
        {
            Task animation = gameAction switch
            {
                MoveCardsGameAction moveCardsGameAction => Get<CardMoverPresenter>().MoveCardWith(moveCardsGameAction),
                ShowHistoryGameAction showHistoryGameAction => Get<ShowHistoryPresenter>().ShowHistory(showHistoryGameAction),
                GainResourceGameAction gainResourceGameAction => Get<TokenMoverPresenter>().GainResource(gainResourceGameAction),
                PayResourceGameAction payResourceGameAction => Get<TokenMoverPresenter>().PayResource(payResourceGameAction),
                GainHintGameAction gainHintsGameAction => Get<TokenMoverPresenter>().GainHints(gainHintsGameAction),
                PayHintGameAction payHintsGameAction => Get<TokenMoverPresenter>().PayHints(payHintsGameAction),
                ShuffleGameAction shuffleGameAction => Get<ShufflePresenter>().Shuffle(shuffleGameAction),
                StatGameAction statGameAction => Get<StatUpdatePresenter>().UpdateStat(statGameAction),
                _ => Task.CompletedTask,
            };
            await animation;
        }

        public async Task<Card> StartSelectionWith(InteractableGameAction interactableGameAction) =>
            await Get<InteractablePresenter>().Interact(interactableGameAction);

        private T Get<T>() where T : IPresenter => (T)_allPresenters.First(presenter => presenter is T);
    }
}
