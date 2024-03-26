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

        public override ChallengeToken AncientToken { get; protected set; }
        public override ChallengeToken CultistToken { get; protected set; }
        public override ChallengeToken DangerToken { get; protected set; }
        public override ChallengeToken CreatureToken { get; protected set; }
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

        public override void PrepareChallengeTokens()
        {
            AncientToken = new ChallengeToken(ChallengeTokenType.Ancient);
            CultistToken = new ChallengeToken(ChallengeTokenType.Cultist);
            DangerToken = new ChallengeToken(ChallengeTokenType.Danger);
            CreatureToken = new ChallengeToken(ChallengeTokenType.Creature);
        }
    }
}

