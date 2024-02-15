using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class SceneCORE1 : Scene
    {
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        private CardPlace Studio => _cardsProvider.GetCard<CardPlace>("01111");
        private Card FirstPlot => _cardsProvider.GetCard("01105");
        private Card FirstGoal => _cardsProvider.GetCard("01108");
        private Card Lita => _cardsProvider.GetCard("01117");
        private Card GhoulPriest => _cardsProvider.GetCard("01116");
        private List<Card> RealDangerCards => Info.DangerCards.Except(new Card[] { Lita, GhoulPriest }).ToList();

        /*******************************************************************/
        public async override Task PrepareScene()
        {
            await _gameActionFactory.Create(new ShowHistoryGameAction(Info.Description));
            await _gameActionFactory.Create(new MoveCardsGameAction(FirstPlot, PlotZone));
            await _gameActionFactory.Create(new MoveCardsGameAction(FirstGoal, GoalZone));
            RealDangerCards.ForEach(card => card.TurnDown(true));
            await _gameActionFactory.Create(new MoveCardsGameAction(RealDangerCards, DangerDeckZone));
            await _gameActionFactory.Create(new MoveCardsGameAction(Studio, PlaceZone[0, 3]));
            await _gameActionFactory.Create(new MoveInvestigatorGameAction(_investigatorsProvider.AllInvestigators, Studio));
        }
    }
}

