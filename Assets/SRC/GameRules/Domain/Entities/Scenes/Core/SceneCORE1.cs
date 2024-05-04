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
            CreateReaction<EliminateInvestigatorGameAction>(InvestigatorsLooseCondition, InvestigatorsLooseLogic, isAtStart: false);
            await _gameActionsProvider.Create(new ShowHistoryGameAction(Info.Description));
            await _gameActionsProvider.Create(new PlacePlotGameAction(FirstPlot));
            await _gameActionsProvider.Create(new PlaceGoalGameAction(FirstGoal));
            await _gameActionsProvider.Create(new MoveCardsGameAction(RealDangerCards, DangerDeckZone, isFaceDown: true));
            await _gameActionsProvider.Create(new MoveCardsGameAction(Study, PlaceZone[0, 3]));
            await _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.AllInvestigatorsInPlay, Study));
        }

        private async Task InvestigatorsLooseLogic(EliminateInvestigatorGameAction action)
        {
            await _gameActionsProvider.Create(new FinalizeGameAction(Resolutions[0])); //TODO: Debe ser corregido, la resolucion esta mal diseñada
        }

        private bool InvestigatorsLooseCondition(EliminateInvestigatorGameAction action)
        {
            if (_investigatorsProvider.AllInvestigatorsInPlay.Count() > 0) return false;
            return true;
        }

        /*******************************************************************/
        protected override void PrepareChallengeTokens()
        {
            base.PrepareChallengeTokens();
            {
                CreatureToken = new ChallengeToken(ChallengeTokenType.Creature, value: CreatureValue, effect: CreatureEffect, description: Info.CreatureTokenDescriptionNormal);
                CultistToken = new ChallengeToken(ChallengeTokenType.Cultist, value: CultistValue, effect: CultistEffect, description: Info.CultistTokenDescriptionNormal);
                DangerToken = new ChallengeToken(ChallengeTokenType.Danger, value: DangerValue, effect: DangerEffect, description: Info.DangerTokenDescriptionNormal);
            }
        }

        private int CreatureValue()
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                return CreatureNormalValue();
            else
                return CreatureHardValue();
        }

        private async Task CreatureEffect()
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                await CreatureNormalEffect();
            else
                await CreatureHardEffect();
        }

        private int CultistValue()
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                return CultistNormalValue();
            else return CultistHardValue();
        }

        private async Task CultistEffect()
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                await CultistNormalEffect();
            else await CultistHardEffect();
        }

        private int DangerValue()
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                return DangerNormalValue();
            else return DangerHardValue();
        }

        private async Task DangerEffect()
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                await DangerNormalEffect();
            else await DangerHardEffect();
        }


        private int CreatureNormalValue() => _gameActionsProvider.CurrentChallenge.ActiveInvestigator.CreaturesInSamePlace
            .Where(creature => creature.Tags.Contains(Tag.Ghoul)).Count() * -1;
        private async Task CreatureNormalEffect() => await Task.CompletedTask;

        private int CultistNormalValue() => -1;
        private async Task CultistNormalEffect()
        {
            _gameActionsProvider.CurrentChallenge.FailEffects.Add(TakeOneFear);
            await Task.CompletedTask;

            /*******************************************************************/
            async Task TakeOneFear() =>
            await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(
                _gameActionsProvider.CurrentChallenge.ActiveInvestigator,
                _gameActionsProvider.CurrentChallenge.CardToChallenge,
                amountFear: 1));
        }

        private int DangerNormalValue() => -2;
        private async Task DangerNormalEffect()
        {
            if (!_gameActionsProvider.CurrentChallenge.ActiveInvestigator.CreaturesInSamePlace
                .Any(creature => creature.Tags.Contains(Tag.Ghoul))) return;

            await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(
            _gameActionsProvider.CurrentChallenge.ActiveInvestigator,
            _gameActionsProvider.CurrentChallenge.CardToChallenge,
            amountDamage: 1));
        }

        private int CreatureHardValue() => -2;
        private async Task CreatureHardEffect()
        {
            CreateReaction<ChallengePhaseGameAction>(condition: DrawGhoulCondition, logic: DrawGhoul, isAtStart: false);
            await Task.CompletedTask;

            /*******************************************************************/
            async Task DrawGhoul(ChallengePhaseGameAction challengePhaseGameAction)
            {
                Card ghoul = DangerDeckZone.Cards.Concat(DangerDiscardZone.Cards).FirstOrDefault(card => card.Tags.Contains(Tag.Ghoul));

                await _gameActionsProvider.Create(new DrawGameAction(_gameActionsProvider.CurrentChallenge.ActiveInvestigator, ghoul));
                await _gameActionsProvider.Create(new ShuffleGameAction(DangerDeckZone));
            }

            bool DrawGhoulCondition(ChallengePhaseGameAction challengePhaseGameAction)
            {
                RemoveReaction<ChallengePhaseGameAction>(DrawGhoul);
                if (challengePhaseGameAction.IsSuccessful ?? true) return false;
                return true;
            }
        }

        private int CultistHardValue() => 0;
        private async Task CultistHardEffect()
        {
            _gameActionsProvider.CurrentChallenge.FailEffects.Add(TakeTwoFear);
            await _gameActionsProvider.Create(new RevealRandomChallengeTokenGameAction());

            /*******************************************************************/
            async Task TakeTwoFear() =>
            await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(
                _gameActionsProvider.CurrentChallenge.ActiveInvestigator,
                _gameActionsProvider.CurrentChallenge.CardToChallenge,
                amountFear: 2));
        }

        private int DangerHardValue() => -4;
        private async Task DangerHardEffect()
        {
            if (!_gameActionsProvider.CurrentChallenge.ActiveInvestigator.CreaturesInSamePlace
               .Any(creature => creature.Tags.Contains(Tag.Ghoul))) return;
            await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(
            _gameActionsProvider.CurrentChallenge.ActiveInvestigator,
            _gameActionsProvider.CurrentChallenge.CardToChallenge,
            amountDamage: 1, amountFear: 1));
        }

        /*******************************************************************/
        protected override async Task Resolution0()
        {
            _chaptersProvider.CurrentChapter.CampaignRegister(CORERegister.HouseUp, true);
            _chaptersProvider.CurrentChapter.CampaignRegister(CORERegister.PriestGhoulLive, true);
            await _gameActionsProvider.Create(new AddRequerimentCardGameAction(_investigatorsProvider.Leader, Lita));
            await _gameActionsProvider.Create(new MoveCardsGameAction(CurrentGoal, _chaptersProvider.CurrentScene.VictoryZone));
            await _gameActionsProvider.Create(new GainSceneXpGameAction());
        }

        protected override async Task Resolution1()
        {
            _chaptersProvider.CurrentChapter.CampaignRegister(CORERegister.HouseUp, false);
            await _gameActionsProvider.Create(new AddRequerimentCardGameAction(_investigatorsProvider.Leader, Lita));
            await _gameActionsProvider.Create(new IncrementStatGameAction(_investigatorsProvider.Leader.Shock, 1));
            await _gameActionsProvider.Create(new MoveCardsGameAction(CurrentGoal, _chaptersProvider.CurrentScene.VictoryZone));
            await _gameActionsProvider.Create(new GainSceneXpGameAction());
        }

        protected override async Task Resolution2()
        {
            _chaptersProvider.CurrentChapter.CampaignRegister(CORERegister.HouseUp, true);
            await _gameActionsProvider.Create(new IncrementStatGameAction(_investigatorsProvider.Leader.Xp, 1));
            await _gameActionsProvider.Create(new MoveCardsGameAction(CurrentGoal, _chaptersProvider.CurrentScene.VictoryZone));
            await _gameActionsProvider.Create(new GainSceneXpGameAction());
        }

        protected override async Task Resolution3()
        {
            _chaptersProvider.CurrentChapter.CampaignRegister(CORERegister.LitaGoAway, true);
            _chaptersProvider.CurrentChapter.CampaignRegister(CORERegister.HouseUp, true);
            _chaptersProvider.CurrentChapter.CampaignRegister(CORERegister.PriestGhoulLive, true);
            await Task.CompletedTask;
            //TODO: continue
        }
    }
}

