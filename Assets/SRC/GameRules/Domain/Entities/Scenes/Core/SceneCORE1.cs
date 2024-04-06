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
        [Inject] private readonly ChaptersProvider _chaptersProvider;

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
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
            {
                FailToken = new ChallengeToken(ChallengeTokenType.Fail, value: FailValue, effect: FailEffect);
                CultistToken = new ChallengeToken(ChallengeTokenType.Cultist, description: Info.CultistTokenDescriptionNormal);
                DangerToken = new ChallengeToken(ChallengeTokenType.Danger, description: Info.DangerTokenDescriptionNormal);
                CreatureToken = new ChallengeToken(ChallengeTokenType.Creature, description: Info.CreatureTokenDescriptionNormal);
            }
            else if (_chaptersProvider.CurrentDificulty == Dificulty.Hard || _chaptersProvider.CurrentDificulty == Dificulty.Expert)
            {
                FailToken = new ChallengeToken(ChallengeTokenType.Fail, value: FailValue, effect: FailEffect);
                CultistToken = new ChallengeToken(ChallengeTokenType.Cultist, description: Info.CultistTokenDescriptionHard);
                DangerToken = new ChallengeToken(ChallengeTokenType.Danger, description: Info.DangerTokenDescriptionHard);
                CreatureToken = new ChallengeToken(ChallengeTokenType.Creature, description: Info.CreatureTokenDescriptionHard);
            }
        }

        private int FailValue() => _gameActionsProvider.CurrentChallenge.TotalChallengeValue * -1;

        private async Task FailEffect()
        {
            _gameActionsProvider.CurrentChallenge.IsAutoFail = true;
            await Task.CompletedTask;
        }
    }
}

