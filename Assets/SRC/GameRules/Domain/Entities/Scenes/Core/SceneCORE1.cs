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
        public override void Init()
        {
            base.Init();
            ParleyLita();
        }

        private void ParleyLita()
        {
            Lita.CreateActivation(1, ParleyActivate, ParleyConditionToActivate, PlayActionType.Parley, "Activation_Card01117");

            async Task ParleyActivate(Investigator activeInvestigator)
            {
                await _gameActionsProvider.Create<ParleyGameAction>().SetWith(TakeLita).Execute();

                /*******************************************************************/
                async Task TakeLita() => await _gameActionsProvider.Create<ChallengePhaseGameAction>()
                    .SetWith(activeInvestigator.Intelligence, 4, "Parley with Lita", cardToChallenge: Lita, ParleySucceed, null)
                    .Execute();

                async Task ParleySucceed() => await _gameActionsProvider.Create<MoveCardsGameAction>()
                    .SetWith(Lita, activeInvestigator.AidZone).Execute();
            }

            bool ParleyConditionToActivate(Investigator activeInvestigator)
            {
                if (activeInvestigator.AvatarCard.CurrentZone != Lita.CurrentZone) return false;
                return true;
            }
        }

        public async override Task PrepareScene()
        {
            await _gameActionsProvider.Create<ShowHistoryGameAction>().SetWith(Descriptions[0]).Execute();
            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(Study, GetPlaceZone(0, 3)).Execute();
            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(StartDeckDangerCards, DangerDeckZone, isFaceDown: true).Execute();
            await _gameActionsProvider.Create<ShuffleGameAction>().SetWith(DangerDeckZone).Execute();
            await _gameActionsProvider.Create<PlacePlotGameAction>().SetWith(FirstPlot).Execute();
            await _gameActionsProvider.Create<PlaceGoalGameAction>().SetWith(FirstGoal).Execute();
            await _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.AllInvestigatorsInPlay, Study).Execute();
        }

        /*******************************************************************/
        protected override void PrepareChallengeTokens()
        {
            CreatureToken = new ChallengeToken(ChallengeTokenType.Creature, value: CreatureValue, effect: CreatureEffect, description: CreatureTokenDescriptionNormal);
            CultistToken = new ChallengeToken(ChallengeTokenType.Cultist, value: CultistValue, effect: CultistEffect, description: CultistTokenDescriptionNormal);
            DangerToken = new ChallengeToken(ChallengeTokenType.Danger, value: DangerValue, effect: DangerEffect, description: DangerTokenDescriptionNormal);
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

                    await _gameActionsProvider.Create<DrawGameAction>().SetWith(_gameActionsProvider.CurrentChallenge.ActiveInvestigator, ghoul).Execute();
                    await _gameActionsProvider.Create<ShuffleGameAction>().SetWith(DangerDeckZone).Execute();
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
                await _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(_gameActionsProvider.CurrentChallenge.ActiveInvestigator, _gameActionsProvider.CurrentChallenge.CardToChallenge, amountFear: 1).Execute();
            }
            async Task CultistHardEffect()
            {
                _gameActionsProvider.CurrentChallenge.FailEffects.Add(TakeTwoFear);
                await _gameActionsProvider.Create<RevealRandomChallengeTokenGameAction>().SetWith(investigator).Execute();

                /*******************************************************************/
                async Task TakeTwoFear() =>
                await _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(_gameActionsProvider.CurrentChallenge.ActiveInvestigator, _gameActionsProvider.CurrentChallenge.CardToChallenge, amountFear: 2).Execute();
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

                await _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(_gameActionsProvider.CurrentChallenge.ActiveInvestigator, _gameActionsProvider.CurrentChallenge.CardToChallenge, amountDamage: 1).Execute();
            }
            async Task DangerHardEffect()
            {
                if (!_gameActionsProvider.CurrentChallenge.ActiveInvestigator.CreaturesInSamePlace
                   .Any(creature => creature.Tags.Contains(Tag.Ghoul))) return;
                await _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(_gameActionsProvider.CurrentChallenge.ActiveInvestigator, _gameActionsProvider.CurrentChallenge.CardToChallenge, amountDamage: 1, amountFear: 1).Execute();
            }
        }

        /*******************************************************************/
        protected override async Task Resolution0()
        {
            await _gameActionsProvider.Create<RegisterChapterGameAction>().SetWith(CORERegister.HouseUp, true).Execute();
            await _gameActionsProvider.Create<RegisterChapterGameAction>().SetWith(CORERegister.PriestGhoulLive, true).Execute();
            if (Lita.ControlOwner != null)
                await _gameActionsProvider.Create<AddRequerimentCardGameAction>().SetWith(Lita.ControlOwner, Lita).Execute();
            await _gameActionsProvider.Create<GainSceneXpGameAction>().Execute();
        }

        protected override async Task Resolution1()
        {
            await _gameActionsProvider.Create<RegisterChapterGameAction>().SetWith(CORERegister.HouseUp, false).Execute();
            if (Lita.ControlOwner != null)
                await _gameActionsProvider.Create<AddRequerimentCardGameAction>().SetWith(Lita.ControlOwner, Lita).Execute();
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(_investigatorsProvider.Leader.Shock, 1).Execute();
            await _gameActionsProvider.Create<GainSceneXpGameAction>().Execute();
        }

        protected override async Task Resolution2()
        {
            await _gameActionsProvider.Create<RegisterChapterGameAction>().SetWith(CORERegister.HouseUp, true).Execute();
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(_investigatorsProvider.Leader.Xp, 1).Execute();
            await _gameActionsProvider.Create<GainSceneXpGameAction>().Execute();
        }

        protected override async Task Resolution3()
        {
            await _gameActionsProvider.Create<RegisterChapterGameAction>().SetWith(CORERegister.LitaGoAway, true).Execute();
            await _gameActionsProvider.Create<RegisterChapterGameAction>().SetWith(CORERegister.HouseUp, true).Execute();
            await _gameActionsProvider.Create<RegisterChapterGameAction>().SetWith(CORERegister.PriestGhoulLive, true).Execute();
            await Task.CompletedTask;
            //TODO: continue
        }
    }
}

