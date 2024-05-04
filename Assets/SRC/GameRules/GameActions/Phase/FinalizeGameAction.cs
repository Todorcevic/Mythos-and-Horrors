using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class FinalizeGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly IPresenter<FinalizeGameAction> _finalizePresenter;

        public Resolution Resolution { get; }

        private IEnumerable<CardPlace> PlaceCardsWithXP => _chaptersProvider.CurrentScene.Info.PlaceCards
            .Where(cardPlace => cardPlace.IsVictory && cardPlace.IsInPlay && cardPlace.Revealed.IsActive && cardPlace.Hints.Value < 1);

        /*******************************************************************/
        public FinalizeGameAction(Resolution resolution)
        {
            Resolution = resolution;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new MoveCardsGameAction(PlaceCardsWithXP, _chaptersProvider.CurrentScene.VictoryZone));
            await _gameActionsProvider.Create(new ShowHistoryGameAction(Resolution.History));
            await Resolution.Logic.Invoke();
            await _finalizePresenter.PlayAnimationWith(this);
        }
    }
}
