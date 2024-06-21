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
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;

        public CardPlace Study => _cardsProvider.GetCard<Card01111>();
        public CardPlace Hallway => _cardsProvider.GetCard<Card01112>();
        public CardPlace Attic => _cardsProvider.GetCard<Card01113>();
        public CardPlace Cellar => _cardsProvider.GetCard<Card01114>();
        public CardPlace Parlor => _cardsProvider.GetCard<Card01115>();

        public CardSupply Lita => _cardsProvider.GetCard<Card01117>();
        public CardCreature GhoulSecuaz => _cardsProvider.GetCard<Card01160>();
        public CardCreature GhoulVoraz => _cardsProvider.GetCard<Card01161>();
        public CardCreature GhoulGelid => _cardsProvider.GetCard<Card01119>();
        public CardCreature GhoulPriest => _cardsProvider.GetCard<Card01116>();

        public override IEnumerable<Card> StartDeckDangerCards => DangerCards.Except(new Card[] { Lita, GhoulPriest });

        /*******************************************************************/
        public async override Task PrepareScene()
        {
            await _gameActionsProvider.Create(new ShowHistoryGameAction(Descriptions[0]));
            await _gameActionsProvider.Create(new MoveCardsGameAction(Study, GetPlaceZone(0, 3)));
            await _gameActionsProvider.Create(new MoveCardsGameAction(StartDeckDangerCards, DangerDeckZone, isFaceDown: true));
            await _gameActionsProvider.Create(new ShuffleGameAction(DangerDeckZone));
            await _gameActionsProvider.Create(new PlacePlotGameAction(FirstPlot));
            await _gameActionsProvider.Create(new PlaceGoalGameAction(FirstGoal));
            await _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.AllInvestigatorsInPlay, Study));
        }

        /*******************************************************************/
        protected override void PrepareChallengeTokens()
        {
            {
                CreatureToken = new ChallengeToken(ChallengeTokenType.Creature, value: CreatureValue, effect: CreatureEffect, description: CreatureTokenDescriptionNormal);
                CultistToken = new ChallengeToken(ChallengeTokenType.Cultist, value: CultistValue, effect: CultistEffect, description: CultistTokenDescriptionNormal);
                DangerToken = new ChallengeToken(ChallengeTokenType.Danger, value: DangerValue, effect: DangerEffect, description: DangerTokenDescriptionNormal);
            }
        }

        private int CreatureValue(Investigator investigator)
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                return CreatureNormalValue();
            else return CreatureHardValue();

            /*******************************************************************/
            int CreatureNormalValue() => _gameActionsProvider.CurrentChallenge.ActiveInvestigator.CreaturesInSamePlace
                .Where(creature => creature.Tags.Contains(Tag.Ghoul)).Count() * -1;
            int CreatureHardValue() => -2;
        }

        private async Task CreatureEffect(Investigator investigator)
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                await CreatureNormalEffect();
            else await CreatureHardEffect();

            /*******************************************************************/
            async Task CreatureNormalEffect() => await Task.CompletedTask;
            async Task CreatureHardEffect()
            {
                Reaction<ChallengePhaseGameAction> DrawGhoulReaction = null;
                DrawGhoulReaction = _reactionablesProvider.CreateReaction<ChallengePhaseGameAction>(condition: DrawGhoulCondition, logic: DrawGhoul, GameActionTime.After);
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
                    _reactionablesProvider.RemoveReaction(DrawGhoulReaction);
                    if (challengePhaseGameAction.IsSucceed) return false;
                    return true;
                }
            }
        }

        private int CultistValue(Investigator investigator)
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                return CultistNormalValue();
            else return CultistHardValue();

            /*******************************************************************/
            int CultistNormalValue() => -1;
            int CultistHardValue() => 0;
        }

        private async Task CultistEffect(Investigator investigator)
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                await CultistNormalEffect();
            else await CultistHardEffect();

            /*******************************************************************/
            async Task CultistNormalEffect()
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
            async Task CultistHardEffect()
            {
                _gameActionsProvider.CurrentChallenge.FailEffects.Add(TakeTwoFear);
                await _gameActionsProvider.Create(new RevealRandomChallengeTokenGameAction(investigator));

                /*******************************************************************/
                async Task TakeTwoFear() =>
                await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(
                    _gameActionsProvider.CurrentChallenge.ActiveInvestigator,
                    _gameActionsProvider.CurrentChallenge.CardToChallenge,
                    amountFear: 2));
            }
        }

        private int DangerValue(Investigator investigator)
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                return DangerNormalValue();
            else return DangerHardValue();

            /*******************************************************************/
            int DangerNormalValue() => -2;
            int DangerHardValue() => -4;
        }

        private async Task DangerEffect(Investigator investigator)
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                await DangerNormalEffect();
            else await DangerHardEffect();

            /*******************************************************************/
            async Task DangerNormalEffect()
            {
                if (!_gameActionsProvider.CurrentChallenge.ActiveInvestigator.CreaturesInSamePlace
                    .Any(creature => creature.Tags.Contains(Tag.Ghoul))) return;

                await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(
                _gameActionsProvider.CurrentChallenge.ActiveInvestigator,
                _gameActionsProvider.CurrentChallenge.CardToChallenge,
                amountDamage: 1));
            }
            async Task DangerHardEffect()
            {
                if (!_gameActionsProvider.CurrentChallenge.ActiveInvestigator.CreaturesInSamePlace
                   .Any(creature => creature.Tags.Contains(Tag.Ghoul))) return;
                await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(
                _gameActionsProvider.CurrentChallenge.ActiveInvestigator,
                _gameActionsProvider.CurrentChallenge.CardToChallenge,
                amountDamage: 1, amountFear: 1));
            }
        }

        /*******************************************************************/
        protected override async Task Resolution0()
        {
            await _gameActionsProvider.Create(new RegisterChapterGameAction(CORERegister.HouseUp, true));
            await _gameActionsProvider.Create(new RegisterChapterGameAction(CORERegister.PriestGhoulLive, true));
            if (Lita.ControlOwner != null) await _gameActionsProvider.Create(new AddRequerimentCardGameAction(Lita.ControlOwner, Lita));
            await _gameActionsProvider.Create(new GainSceneXpGameAction());
        }

        protected override async Task Resolution1()
        {
            await _gameActionsProvider.Create(new RegisterChapterGameAction(CORERegister.HouseUp, false));
            if (Lita.ControlOwner != null) await _gameActionsProvider.Create(new AddRequerimentCardGameAction(Lita.ControlOwner, Lita));
            await _gameActionsProvider.Create(new IncrementStatGameAction(_investigatorsProvider.Leader.Shock, 1));
            await _gameActionsProvider.Create(new GainSceneXpGameAction());
        }

        protected override async Task Resolution2()
        {
            await _gameActionsProvider.Create(new RegisterChapterGameAction(CORERegister.HouseUp, true));
            await _gameActionsProvider.Create(new IncrementStatGameAction(_investigatorsProvider.Leader.Xp, 1));
            await _gameActionsProvider.Create(new GainSceneXpGameAction());
        }

        protected override async Task Resolution3()
        {
            await _gameActionsProvider.Create(new RegisterChapterGameAction(CORERegister.LitaGoAway, true));
            await _gameActionsProvider.Create(new RegisterChapterGameAction(CORERegister.HouseUp, true));
            await _gameActionsProvider.Create(new RegisterChapterGameAction(CORERegister.PriestGhoulLive, true));
            await Task.CompletedTask;
            //TODO: continue
        }
    }
}

