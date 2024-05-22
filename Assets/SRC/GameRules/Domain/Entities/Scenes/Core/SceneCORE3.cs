using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class SceneCORE3 : Scene
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;

        public CardPlace MainPath => _cardsProvider.GetCard<Card01149>();
        public CardPlace Forest1 => _cardsProvider.GetCard<Card01150>();
        public CardPlace Forest2 => _cardsProvider.GetCard<Card01151>();
        public CardPlace Forest3 => _cardsProvider.GetCard<Card01152>();
        public CardPlace Forest4 => _cardsProvider.GetCard<Card01153>();
        public CardPlace Forest5 => _cardsProvider.GetCard<Card01154>();
        public CardPlace Forest6 => _cardsProvider.GetCard<Card01155>();
        public CardPlace Ritual => _cardsProvider.GetCard<Card01156>();
        public CardCreature GhoulPriest => _cardsProvider.GetCard<Card01116>();
        public CardCreature Urmodoth => _cardsProvider.GetCard<Card01157>();
        public IEnumerable<Card> Haunteds => _cardsProvider.GetCards<Card01598>();
        public IEnumerable<Card> Hastur => _cardsProvider.GetCards<Card01175>().Cast<Card>().Concat(_cardsProvider.GetCards<Card01176>());
        public IEnumerable<Card> Yog => _cardsProvider.GetCards<Card01177>().Cast<Card>().Concat(_cardsProvider.GetCards<Card01178>());
        public IEnumerable<Card> Shub => _cardsProvider.GetCards<Card01179>().Cast<Card>().Concat(_cardsProvider.GetCards<Card01180>());
        public IEnumerable<Card> Cthulhu => _cardsProvider.GetCards<Card01181>().Cast<Card>().Concat(_cardsProvider.GetCards<Card01182>());
        public IEnumerable<Card> AllAgents => Hastur.Concat(Yog).Concat(Shub).Concat(Cthulhu);
        public override IEnumerable<Card> StartDeckDangerCards => _chaptersProvider.CurrentChapter.IsRegistered(CORERegister.PriestGhoulLive) ?
            Info.DangerCards.Except(AllAgents.Except(AgentSelected)).Except(Haunteds) :
            Info.DangerCards.Except(AllAgents.Except(AgentSelected)).Except(Haunteds).Except(new[] { GhoulPriest });

        public int AmountInterrogate =>
            (_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.DrewInterrogate) ? 1 : 0) +
            (_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.HermanInterrogate) ? 1 : 0) +
            (_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.PeterInterrogate) ? 1 : 0) +
            (_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.RuthInterrogate) ? 1 : 0) +
            (_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.VictoriaInterrogate) ? 1 : 0) +
            (_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.MaskedHunterInterrogate) ? 1 : 0);

        public List<CardPlace> Forests => new() { Forest1, Forest2, Forest3, Forest4, Forest5, Forest6 };

        private List<CardPlace> _forestsToPlace;
        public List<CardPlace> ForestsToPlace => _forestsToPlace ??= Forests.Rand(4).ToList();

        private IEnumerable<Card> _agentSelected;
        public IEnumerable<Card> AgentSelected => _agentSelected ??= new List<IEnumerable<Card>> { Hastur, Yog, Shub, Cthulhu }.Rand().ToList();


        /*******************************************************************/
        public override async Task PrepareScene()
        {
            await ShowHistory();
            await PlacePlaces();
            await PlaceDangerDeck();
            await PlacePlotAndGoal();
            await PlaceInvestigators();
            await CheckDiscard();
        }

        /*******************************************************************/
        private async Task ShowHistory()
        {
            await _gameActionsProvider.Create(new ShowHistoryGameAction(Info.Descriptions[0]));
        }

        private async Task PlacePlaces()
        {
            Dictionary<Card, Zone> allPlaces = new()
            {
                { MainPath, PlaceZone[1, 3] },
                { ForestsToPlace[0], PlaceZone[0, 2] },
                { ForestsToPlace[1], PlaceZone[0, 4] },
                { ForestsToPlace[2], PlaceZone[2, 2] },
                { ForestsToPlace[3], PlaceZone[2, 4] },
            };

            await _gameActionsProvider.Create(new MoveCardsGameAction(allPlaces));
        }

        private async Task PlaceDangerDeck()
        {
            await _gameActionsProvider.Create(new MoveCardsGameAction(StartDeckDangerCards, DangerDeckZone, isFaceDown: true));
            await _gameActionsProvider.Create(new ShuffleGameAction(DangerDeckZone));
        }

        private async Task PlacePlotAndGoal()
        {
            await _gameActionsProvider.Create(new PlacePlotGameAction(FirstPlot));
            await _gameActionsProvider.Create(new PlaceGoalGameAction(FirstGoal));

            int totaL = 0;
            if (AmountInterrogate < 2) totaL = 3;
            else if (AmountInterrogate < 4) totaL = 2;
            else if (AmountInterrogate < 6) totaL = 1;
            await _gameActionsProvider.Create(new DecrementStatGameAction(FirstPlot.Eldritch, totaL));
        }

        private async Task CheckDiscard()
        {
            if (_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.IsMidknigh))
                await _gameActionsProvider.Create(new SafeForeach<Investigator>(InvestigatorsWithCards, Discard));

            IEnumerable<Investigator> InvestigatorsWithCards() => _investigatorsProvider.AllInvestigatorsInPlay
                .Where(investigator => investigator.HandZone.Cards.Any());

            async Task Discard(Investigator investigator)
            {
                Card cardToDiscard = investigator.HandZone.Cards.Rand();
                if (cardToDiscard == null) return;
                await _gameActionsProvider.Create(new DiscardGameAction(cardToDiscard));
                cardToDiscard = investigator.HandZone.Cards.Rand();
                if (cardToDiscard == null) return;
                await _gameActionsProvider.Create(new DiscardGameAction(cardToDiscard));
            }
        }

        private async Task PlaceInvestigators()
        {
            await _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.AllInvestigators, MainPath));
        }

        /*******************************************************************/
        protected override async Task Resolution0()
        {
            await _gameActionsProvider.Create(new RegisterChapterGameAction(CORERegister.UmordhothWin, true));
            await _gameActionsProvider.Create(new SafeForeach<Investigator>(() => _investigatorsProvider.AllInvestigatorsInPlay, Defeated));

            async Task Defeated(Investigator investigator)
            {
                await _gameActionsProvider.Create(new UpdateStatesGameAction(investigator.Defeated, true));
            }
        }

        protected override async Task Resolution1()
        {
            await _gameActionsProvider.Create(new RegisterChapterGameAction(CORERegister.UmordhothWin, false));
            await TakeShock();
            await _gameActionsProvider.Create(new GainSceneXpGameAction());

            async Task TakeShock()
            {
                Dictionary<Stat, int> statsWithValues = new();
                foreach (var investigator in _investigatorsProvider.AllInvestigatorsInPlay)
                {
                    statsWithValues.Add(investigator.Shock, 2);
                }
                await _gameActionsProvider.Create(new IncrementStatGameAction(statsWithValues));
            }
        }

        protected override async Task Resolution2()
        {
            await _gameActionsProvider.Create(new RegisterChapterGameAction(CORERegister.UmordhothWin, false));
            await TakeInjuriesAndShock();
            await _gameActionsProvider.Create(new GainSceneXpGameAction());

            async Task TakeInjuriesAndShock()
            {
                Dictionary<Stat, int> statsWithValues = new();
                foreach (var investigator in _investigatorsProvider.AllInvestigatorsInPlay)
                {
                    statsWithValues.Add(investigator.Injury, 2);
                    statsWithValues.Add(investigator.Shock, 2);
                }
                await _gameActionsProvider.Create(new IncrementStatGameAction(statsWithValues));
            }
        }

        protected override async Task Resolution3()
        {
            await _gameActionsProvider.Create(new RegisterChapterGameAction(CORERegister.LitaSacrifice, true));
            await TakeInjuriesAndShock();
            await TakeFlaw();
            await _gameActionsProvider.Create(new GainSceneXpGameAction());

            async Task TakeInjuriesAndShock()
            {
                Dictionary<Stat, int> statsWithValues = new();
                foreach (var investigator in _investigatorsProvider.AllInvestigatorsInPlay)
                {
                    statsWithValues.Add(investigator.Injury, 2);
                    statsWithValues.Add(investigator.Shock, 2);
                }
                await _gameActionsProvider.Create(new IncrementStatGameAction(statsWithValues));
            }

            async Task TakeFlaw()
            {
                await Task.CompletedTask; //TODO: Implement this method Una carta IFlaw a diseñar (LitaSacrifice) para todos los investigadores
            }
        }

        /*******************************************************************/
        protected override void PrepareChallengeTokens()
        {
            {
                CreatureToken = new ChallengeToken(ChallengeTokenType.Creature, value: CreatureValue, effect: CreatureEffect, description: Info.CreatureTokenDescriptionNormal);
                CultistToken = new ChallengeToken(ChallengeTokenType.Cultist, value: CultistValue, effect: CultistEffect, description: Info.CultistTokenDescriptionNormal);
                DangerToken = new ChallengeToken(ChallengeTokenType.Danger, value: DangerValue, effect: DangerEffect, description: Info.DangerTokenDescriptionNormal);
                AncientToken = new ChallengeToken(ChallengeTokenType.Ancient, value: AncientValue, effect: AncientEffect, description: Info.DangerTokenDescriptionNormal);
            }
        }

        private int CreatureValue(Investigator investigator)
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                return CreatureNormalValue();
            else return CreatureHardValue();

            /*******************************************************************/
            int CreatureNormalValue() => _cardsProvider.GetCards<CardCreature>()
                .Where(creature => creature.IsInPlay && creature.HasThisTag(Tag.Monster)).Count() * -1
;

            int CreatureHardValue() => -3;
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
                _reactionablesProvider.CreateReaction<ChallengePhaseGameAction>(condition: DrawMonsterCondition, logic: DrawMonster, isAtStart: false);
                await Task.CompletedTask;

                /*******************************************************************/
                async Task DrawMonster(ChallengePhaseGameAction challengePhaseGameAction)
                {
                    Card monster = DangerDeckZone.Cards.Concat(DangerDiscardZone.Cards).FirstOrDefault(card => card.Tags.Contains(Tag.Monster));

                    await _gameActionsProvider.Create(new DrawGameAction(_gameActionsProvider.CurrentChallenge.ActiveInvestigator, monster));
                    await _gameActionsProvider.Create(new ShuffleGameAction(DangerDeckZone));
                }

                bool DrawMonsterCondition(ChallengePhaseGameAction challengePhaseGameAction)
                {
                    _reactionablesProvider.RemoveReaction<ChallengePhaseGameAction>(DrawMonster);
                    if (challengePhaseGameAction.IsSuccessful ?? true) return false;
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
            int CultistNormalValue() => -2;
            int CultistHardValue() => -4;
        }

        private async Task CultistEffect(Investigator investigator)
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                await CultistNormalEffect();
            else await CultistHardEffect();

            /*******************************************************************/
            async Task CultistNormalEffect()
            {
                IEldritchable nearestCreature = _gameActionsProvider.CurrentChallenge.ActiveInvestigator.NearestCreatures
                    .OfType<IEldritchable>().FirstOrDefault();
                if (nearestCreature != null) await _gameActionsProvider.Create(new IncrementStatGameAction(nearestCreature.Eldritch, 1));
            }

            async Task CultistHardEffect()
            {
                IEldritchable nearestCreature = _gameActionsProvider.CurrentChallenge.ActiveInvestigator.NearestCreatures
                  .OfType<IEldritchable>().FirstOrDefault();
                if (nearestCreature != null) await _gameActionsProvider.Create(new IncrementStatGameAction(nearestCreature.Eldritch, 2));
            }
        }

        private int DangerValue(Investigator investigator)
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                return DangerNormalValue();
            else return DangerHardValue();

            /*******************************************************************/
            int DangerNormalValue() => -3;
            int DangerHardValue() => -5;
        }

        private async Task DangerEffect(Investigator investigator)
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                await DangerNormalEffect();
            else await DangerHardEffect();

            /*******************************************************************/
            async Task DangerNormalEffect()
            {
                if (!_gameActionsProvider.CurrentChallenge.ActiveInvestigator.CreaturesInSamePlace.Any()) return;
                await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(
                    _gameActionsProvider.CurrentChallenge.ActiveInvestigator,
                     _gameActionsProvider.CurrentChallenge.CardToChallenge, amountDamage: 1));
            }

            async Task DangerHardEffect()
            {
                if (!_gameActionsProvider.CurrentChallenge.ActiveInvestigator.CreaturesInSamePlace.Any()) return;
                await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(
                    _gameActionsProvider.CurrentChallenge.ActiveInvestigator,
                     _gameActionsProvider.CurrentChallenge.CardToChallenge, amountDamage: 1, amountFear: 1));
            }
        }

        private int AncientValue(Investigator investigator)
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                return AncientNormalValue();
            else return AncientHardValue();

            /*******************************************************************/
            int AncientNormalValue() => -5;

            int AncientHardValue() => -7;
        }

        private Task AncientEffect(Investigator investigator)
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                return AncientNormalEffect();
            else return AncientHardEffect();

            /*******************************************************************/
            async Task AncientNormalEffect()
            {
                if (!_cardsProvider.GetCards<CardCreature>()
                    .Where(creature => creature.IsInPlay && creature.HasThisTag(Tag.AncientOne)).Any()) return;
                await _gameActionsProvider.Create(new RevealRandomChallengeTokenGameAction(investigator));
            }

            async Task AncientHardEffect()
            {
                if (!_cardsProvider.GetCards<CardCreature>()
                   .Where(creature => creature.IsInPlay && creature.HasThisTag(Tag.AncientOne)).Any()) return;
                await _gameActionsProvider.Create(new RevealRandomChallengeTokenGameAction(investigator));
            }
        }
    }
}
