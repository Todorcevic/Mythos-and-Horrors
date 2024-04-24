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

        public CardPlace Study => _cardsProvider.GetCard<Card01111>();
        public CardPlace Hallway => _cardsProvider.GetCard<Card01112>();
        public CardPlace Attic => _cardsProvider.GetCard<Card01113>();
        public CardPlace Cellar => _cardsProvider.GetCard<Card01114>();
        public CardPlace Parlor => _cardsProvider.GetCard<Card01115>();

        public CardSupply Lita => _cardsProvider.GetCard<Card01117>();
        public CardCreature GhoulSecuaz => _cardsProvider.GetCard<Card01160>();
        public CardCreature GhoulVoraz => _cardsProvider.GetCard<Card01161>();
        public CardCreature GhoulPriest => _cardsProvider.GetCard<Card01116>();

        public CardPlot FirstPlot => Info.PlotCards.First();
        public CardGoal FirstGoal => Info.GoalCards.First();

        public IEnumerable<Card> RealDangerCards => Info.DangerCards.Except(new Card[] { Lita, GhoulPriest });

        /*******************************************************************/
        public async override Task PrepareScene()
        {
            await _gameActionsProvider.Create(new ShowHistoryGameAction(Info.Description));
            await _gameActionsProvider.Create(new PlacePlotGameAction(FirstPlot));
            await _gameActionsProvider.Create(new PlaceGoalGameAction(FirstGoal));
            await _gameActionsProvider.Create(new MoveCardsGameAction(RealDangerCards, DangerDeckZone, isFaceDown: true));
            await _gameActionsProvider.Create(new MoveCardsGameAction(Study, PlaceZone[0, 3]));
            await _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.AllInvestigatorsInPlay, Study));
        }

        protected override void PrepareChallengeTokens()
        {
            base.PrepareChallengeTokens();
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
            {
                CultistToken = new ChallengeToken(ChallengeTokenType.Cultist, effect: FailEffect, description: Info.CultistTokenDescriptionNormal);
                DangerToken = new ChallengeToken(ChallengeTokenType.Danger, effect: FailEffect, description: Info.DangerTokenDescriptionNormal);
                CreatureToken = new ChallengeToken(ChallengeTokenType.Creature, effect: FailEffect, description: Info.CreatureTokenDescriptionNormal);
            }
            else if (_chaptersProvider.CurrentDificulty == Dificulty.Hard || _chaptersProvider.CurrentDificulty == Dificulty.Expert)
            {
                CultistToken = new ChallengeToken(ChallengeTokenType.Cultist, effect: FailEffect, description: Info.CultistTokenDescriptionHard);
                DangerToken = new ChallengeToken(ChallengeTokenType.Danger, effect: FailEffect, description: Info.DangerTokenDescriptionHard);
                CreatureToken = new ChallengeToken(ChallengeTokenType.Creature, effect: FailEffect, description: Info.CreatureTokenDescriptionHard);
            }
        }

        private async Task FailEffect() // TODO: Test Effect, must be change
        {
            _gameActionsProvider.CurrentChallenge.IsAutoFail = true;
            await Task.CompletedTask;
        }

        /*******************************************************************/
        public override async Task Resolution0()
        {
            await _gameActionsProvider.Create(new ShowHistoryGameAction(Info.Resolutions[0]));

            _chaptersProvider.CurrentChapter.CampaignRegister(CORERegister.HouseUp, true);
            _chaptersProvider.CurrentChapter.CampaignRegister(CORERegister.PriestGhoulLive, true);
            _investigatorsProvider.Leader.RequerimentCard.Add(Lita);
            int amountXp = VictoryZone.Cards.Sum(card => card.Info.Victory) ?? 0 + 2;
            Dictionary<Stat, int> xp = _investigatorsProvider.AllInvestigatorsInPlay.ToDictionary(investigator => investigator.Xp, investigator => amountXp);
            await _gameActionsProvider.Create(new IncrementStatGameAction(xp));
        }

        public override async Task Resolution1()
        {
            await _gameActionsProvider.Create(new ShowHistoryGameAction(Info.Resolutions[1]));
            _chaptersProvider.CurrentChapter.CampaignRegister(CORERegister.HouseUp, false);
            _investigatorsProvider.Leader.RequerimentCard.Add(Lita);
            await _gameActionsProvider.Create(new IncrementStatGameAction(_investigatorsProvider.Leader.Shock, 1));
            int amountXp = VictoryZone.Cards.Sum(card => card.Info.Victory) ?? 0 + 2;
            Dictionary<Stat, int> xp = _investigatorsProvider.AllInvestigatorsInPlay.ToDictionary(investigator => investigator.Xp, investigator => amountXp);
            await _gameActionsProvider.Create(new IncrementStatGameAction(xp));
        }

        public override async Task Resolution2()
        {
            await _gameActionsProvider.Create(new ShowHistoryGameAction(Info.Resolutions[2]));
            _chaptersProvider.CurrentChapter.CampaignRegister(CORERegister.HouseUp, true);
            await _gameActionsProvider.Create(new IncrementStatGameAction(_investigatorsProvider.Leader.Xp, 1));
            int amountXp = VictoryZone.Cards.Sum(card => card.Info.Victory) ?? 0 + 2;
            Dictionary<Stat, int> xp = _investigatorsProvider.AllInvestigatorsInPlay.ToDictionary(investigator => investigator.Xp, investigator => amountXp);
            await _gameActionsProvider.Create(new IncrementStatGameAction(xp));

        }

        public override async Task Resolution3()
        {
            await _gameActionsProvider.Create(new ShowHistoryGameAction(Info.Resolutions[3]));

            _chaptersProvider.CurrentChapter.CampaignRegister(CORERegister.LitaGoAway, true);
            _chaptersProvider.CurrentChapter.CampaignRegister(CORERegister.HouseUp, true);
            _chaptersProvider.CurrentChapter.CampaignRegister(CORERegister.PriestGhoulLive, true);

            //TODO: continue
        }
    }
}

