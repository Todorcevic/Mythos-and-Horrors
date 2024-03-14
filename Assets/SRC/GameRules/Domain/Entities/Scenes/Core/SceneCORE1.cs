using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class SceneCORE1 : Scene
    {
        [Inject] private readonly GameActionProvider _gameActionFactory;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        private CardPlace Studio => _cardsProvider.GetCard<CardPlace>("01111");
        private CardPlot FirstPlot => Info.PlotCards.First();
        private CardGoal FirstGoal => Info.GoalCards.First();
        private Card Lita => _cardsProvider.GetCard("01117");
        private Card GhoulPriest => _cardsProvider.GetCard("01116");
        private List<Card> RealDangerCards => Info.DangerCards.Except(new Card[] { Lita, GhoulPriest }).ToList();

        /*******************************************************************/
        public async override Task PrepareScene()
        {
            await _gameActionFactory.Create(new ShowHistoryGameAction(Info.Description));
            await _gameActionFactory.Create(new PlacePlotGameAction(FirstPlot));
            await _gameActionFactory.Create(new PlaceGoalGameAction(FirstGoal));
            await _gameActionFactory.Create(new UpdateStatesGameAction(RealDangerCards.Select(card => card.FaceDown).ToList(), true));
            await _gameActionFactory.Create(new MoveCardsGameAction(RealDangerCards, DangerDeckZone));
            await _gameActionFactory.Create(new MoveCardsGameAction(Studio, PlaceZone[0, 3]));
            await _gameActionFactory.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.AllInvestigators, Studio));
        }
    }
}

