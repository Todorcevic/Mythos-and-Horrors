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
        [Inject] private readonly ZonesProvider _zonesProvider;

        private Card Studio => _cardsProvider.GetCard("01111");
        private Card FirstPlot => _cardsProvider.GetCard("01105");
        private Card FirstGoal => _cardsProvider.GetCard("01108");
        private Card Lita => _cardsProvider.GetCard("01117");
        private Card GhoulPriest => _cardsProvider.GetCard("01116");
        private List<Card> RealDangerCards => Info.DangerCards.Except(new Card[] { Lita, GhoulPriest }).ToList();

        /*******************************************************************/
        public async override Task PrepareScene()
        {
            await _gameActionFactory.Create<MoveCardGameAction>().Run(FirstPlot, _zonesProvider.PlotZone);
            await _gameActionFactory.Create<MoveCardGameAction>().Run(FirstGoal, _zonesProvider.GoalZone);
            await _gameActionFactory.Create<MoveCardsGameAction>().Run(RealDangerCards, _zonesProvider.DangerDeckZone);
            await _gameActionFactory.Create<MoveCardGameAction>().Run(Studio, _zonesProvider.PlaceZone[0, 3]);
        }
    }
}
