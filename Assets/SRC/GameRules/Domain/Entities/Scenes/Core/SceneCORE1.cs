using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class SceneCORE1 : Scene
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        private CardPlace Studio => _cardsProvider.GetCard<CardPlace>("01111");
        private CardPlot FirstPlot => Info.PlotCards.First();
        private CardGoal FirstGoal => Info.GoalCards.First();
        private Card Lita => _cardsProvider.GetCard("01117");
        private Card GhoulPriest => _cardsProvider.GetCard("01116");
        private IEnumerable<Card> RealDangerCards => Info.DangerCards.Except(new Card[] { Lita, GhoulPriest });

        /*******************************************************************/
        public async override Task PrepareScene()
        {
            await _gameActionsProvider.Create(new ShowHistoryGameAction(Info.Description));
            await _gameActionsProvider.Create(new PlacePlotGameAction(FirstPlot));
            await _gameActionsProvider.Create(new PlaceGoalGameAction(FirstGoal));
            await _gameActionsProvider.Create(new MoveCardsGameAction(RealDangerCards, DangerDeckZone, isFaceDown: true));
            await _gameActionsProvider.Create(new MoveCardsGameAction(Studio, PlaceZone[0, 3]));
            await _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.AllInvestigatorsInPlay, Studio));
        }
    }
}

