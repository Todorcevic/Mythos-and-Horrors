using ModestTree;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class SceneCORE2 : Scene
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;

        public CardCreature Drew => _cardsProvider.GetCard<Card01137>();
        public CardCreature Herman => _cardsProvider.GetCard<Card01138>();
        public CardCreature Peter => _cardsProvider.GetCard<Card01139>();
        public CardCreature Victoria => _cardsProvider.GetCard<Card01140>();
        public CardCreature Ruth => _cardsProvider.GetCard<Card01141>();
        public CardCreature MaskedHunter => _cardsProvider.GetCard<Card01121b>();
        public CardCreature GhoulPriest => _cardsProvider.GetCard<Card01116>();
        public List<Card> Cultists => new() { Drew, Herman, Peter, Victoria, Ruth };
        public List<Card> AllCultists => new() { Drew, Herman, Peter, Victoria, Ruth, MaskedHunter };
        public CardPlace Home => _cardsProvider.GetCard<Card01124>();
        public CardPlace Fluvial => _cardsProvider.GetCard<Card01125>();
        public CardPlace Hospital => _cardsProvider.GetCard<Card01128>();
        public CardPlace University => _cardsProvider.GetCard<Card01129>();
        public CardPlace East => _cardsProvider.GetCard<Card01132>();
        public CardPlace Graveyard => _cardsProvider.GetCard<Card01133>();
        public CardPlace North => _cardsProvider.GetCard<Card01134>();

        private CardPlace _south;
        public CardPlace South => _south ??= new List<CardPlace> { _cardsProvider.GetCard<Card01126>(), _cardsProvider.GetCard<Card01127>() }.Rand();

        private CardPlace _center;
        public CardPlace Center => _center ??= new List<CardPlace> { _cardsProvider.GetCard<Card01130>(), _cardsProvider.GetCard<Card01131>() }.Rand();

        public override IEnumerable<Card> StartDeckDangerCards => _chaptersProvider.CurrentChapter.IsRegistered(CORERegister.PriestGhoulLive) ?
                     DangerCards.Except(Cultists) :
                     DangerCards.Except(Cultists).Except(new[] { GhoulPriest });

        public IEnumerable<Card01169> Acolits => _cardsProvider.AllCards.OfType<Card01169>();

        /*******************************************************************/
        public override async Task PrepareScene()
        {
            await ShowHistory();
            await PlacePlaces();
            await PlaceDangerDeck();
            await PlaceAcolits();
            await PlacePlotAndGoal();
            await PlaceInvestigators();
        }

        /*******************************************************************/
        private async Task ShowHistory()
        {
            if (_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.LitaGoAway))
                await _gameActionsProvider.Create<ShowHistoryGameAction>().SetWith(Descriptions[0]).Execute();
            else await _gameActionsProvider.Create<ShowHistoryGameAction>().SetWith(Descriptions[1]).Execute();
            await _gameActionsProvider.Create<ShowHistoryGameAction>().SetWith(Descriptions[2]).Execute();
        }

        private async Task PlacePlaces()
        {
            Dictionary<Card, Zone> allPlaces = new()
            {
                { South, GetPlaceZone(0, 3) },
                { Hospital, GetPlaceZone(0, 2) },
                { Graveyard, GetPlaceZone(1, 4) },
                { Fluvial, GetPlaceZone(1, 3) },
                { University, GetPlaceZone(1, 2) },
                { East, GetPlaceZone(2, 4) },
                { Center, GetPlaceZone(2, 3) },
                { North, GetPlaceZone(2, 2) }
            };


            if (_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.HouseUp))
                allPlaces.Add(Home, GetPlaceZone(0, 4));

            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(allPlaces).Execute();
        }

        private async Task PlaceAcolits()
        {
            if (_investigatorsProvider.AllInvestigatorsInPlay.Count() > 1)
                await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(Acolits.ElementAt(0), South.OwnZone).Execute();
            if (_investigatorsProvider.AllInvestigatorsInPlay.Count() > 2)
                await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(Acolits.ElementAt(1), Center.OwnZone).Execute();
            if (_investigatorsProvider.AllInvestigatorsInPlay.Count() > 3)
                await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(Acolits.ElementAt(2), Graveyard.OwnZone).Execute();
        }

        private async Task PlaceDangerDeck()
        {
            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(StartDeckDangerCards, DangerDeckZone, isFaceDown: true).Execute();
            await _gameActionsProvider.Create<ShuffleGameAction>().SetWith(DangerDeckZone).Execute();
        }

        private async Task PlacePlotAndGoal()
        {
            await _gameActionsProvider.Create<PlacePlotGameAction>().SetWith(FirstPlot).Execute();
            await _gameActionsProvider.Create<PlaceGoalGameAction>().SetWith(FirstGoal).Execute();
        }

        private async Task PlaceInvestigators()
        {
            if (_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.HouseUp))
                await _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>()
                    .SetWith(_investigatorsProvider.AllInvestigatorsInPlay, Home).Execute();
            else
                await _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>()
                    .SetWith(_investigatorsProvider.AllInvestigatorsInPlay, Fluvial).Execute();
        }

        /*******************************************************************/
        protected override async Task Resolution0()
        {
            await Resolution1();
        }

        protected override async Task Resolution1()
        {
            await Interrogates();
            await _gameActionsProvider.Create<GainSceneXpGameAction>().Execute();

        }

        protected override async Task Resolution2()
        {
            await Interrogates();
            await _gameActionsProvider.Create<RegisterChapterGameAction>().SetWith(CORERegister.IsMidknigh, true).Execute();
            await _gameActionsProvider.Create<GainSceneXpGameAction>().Execute();
        }

        private async Task Interrogates()
        {
            if (Drew.CurrentZone == VictoryZone)
                await _gameActionsProvider.Create<RegisterChapterGameAction>().SetWith(CORERegister.DrewInterrogate, true).Execute();
            if (Herman.CurrentZone == VictoryZone)
                await _gameActionsProvider.Create<RegisterChapterGameAction>().SetWith(CORERegister.HermanInterrogate, true).Execute();
            if (Peter.CurrentZone == VictoryZone)
                await _gameActionsProvider.Create<RegisterChapterGameAction>().SetWith(CORERegister.PeterInterrogate, true).Execute();
            if (Victoria.CurrentZone == VictoryZone)
                await _gameActionsProvider.Create<RegisterChapterGameAction>().SetWith(CORERegister.VictoriaInterrogate, true).Execute();
            if (Ruth.CurrentZone == VictoryZone)
                await _gameActionsProvider.Create<RegisterChapterGameAction>().SetWith(CORERegister.RuthInterrogate, true).Execute();
            if (MaskedHunter.CurrentZone == VictoryZone)
                await _gameActionsProvider.Create<RegisterChapterGameAction>().SetWith(CORERegister.MaskedHunterInterrogate, true).Execute();
            if (GhoulPriest.CurrentZone == VictoryZone)
                await _gameActionsProvider.Create<RegisterChapterGameAction>().SetWith(CORERegister.PriestGhoulLive, false).Execute();
        }

        /*******************************************************************/
        protected override void PrepareChallengeTokens()
        {
            {
                CreatureToken = new ChallengeToken(ChallengeTokenType.Creature, value: CreatureValue, effect: CreatureEffect, description: (_) => CreatureTokenDescriptionNormal);
                CultistToken = new ChallengeToken(ChallengeTokenType.Cultist, value: CultistValue, effect: CultistEffect, description: (_) => CultistTokenDescriptionNormal);
                DangerToken = new ChallengeToken(ChallengeTokenType.Danger, value: DangerValue, effect: DangerEffect, description: (_) => DangerTokenDescriptionNormal);
            }
        }

        private int CreatureValue(Investigator investigator)
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                return CreatureNormalValue();
            else return CreatureHardValue();

            /*******************************************************************/
            int CreatureNormalValue() => _cardsProvider.GetCards<CardCreature>()
                .Where(creature => creature.IsInPlay.IsTrue && creature.HasThisTag(Tag.Cultist))
                .OfType<IEldritchable>().Select(eldritchable => eldritchable.Eldritch.Value)
                .OrderByDescending(eldritch => eldritch).FirstOrDefault() * -1;

            int CreatureHardValue() => ((CurrentPlot.Info.Eldritch ?? 0) - CurrentPlot.Eldritch.Value +
                _cardsProvider.AllCards.OfType<IEldritchable>().Sum(eldrichable => eldrichable.Eldritch.Value))
                * -1;
        }

        private async Task CreatureEffect(Investigator investigator)
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                await CreatureNormalEffect();
            else await CreatureHardEffect();

            /*******************************************************************/
            async Task CreatureNormalEffect() => await Task.CompletedTask;
            async Task CreatureHardEffect() => await Task.CompletedTask;
        }

        private int CultistValue(Investigator investigator)
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                return CultistNormalValue();
            else return CultistHardValue();

            /*******************************************************************/
            int CultistNormalValue() => -2;
            int CultistHardValue() => -2;
        }

        private async Task CultistEffect(Investigator investigator)
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                await CultistNormalEffect();
            else await CultistHardEffect();

            /*******************************************************************/
            async Task CultistNormalEffect()
            {
                var nearest = _gameActionsProvider.CurrentChallenge.ActiveInvestigator.NearestCreatures.ToList();

                IEldritchable nearestCreature = nearest
                    .Where(creature => creature.HasThisTag(Tag.Cultist)).OfType<IEldritchable>().FirstOrDefault();

                if (nearestCreature != null) await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(nearestCreature.Eldritch, 1).Execute();
            }

            async Task CultistHardEffect()
            {
                Dictionary<Stat, int> allEldrichableStats = _cardsProvider.GetCards<CardCreature>()
                    .Where(creature => creature.IsInPlay.IsTrue && creature.HasThisTag(Tag.Cultist))
                        .OfType<IEldritchable>().ToDictionary(cultist => cultist.Eldritch, cultist => 1);

                if (allEldrichableStats.Any()) await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(allEldrichableStats).Execute();
                else await _gameActionsProvider.Create<RevealRandomChallengeTokenGameAction>().SetWith(investigator).Execute();
            }
        }

        private int DangerValue(Investigator investigator)
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                return DangerNormalValue();
            else return DangerHardValue();

            /*******************************************************************/
            int DangerNormalValue() => -3;
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
                _gameActionsProvider.CurrentChallenge.FailEffects.Add(DropHint);
                await Task.CompletedTask;

                /*******************************************************************/
                async Task DropHint() =>
                                await _gameActionsProvider.Create<DropKeyGameAction>().SetWith(_gameActionsProvider.CurrentChallenge.ActiveInvestigator,
                    _gameActionsProvider.CurrentChallenge.ActiveInvestigator.CurrentPlace.Hints, amount: 1).Execute();
            }

            async Task DangerHardEffect()
            {
                _gameActionsProvider.CurrentChallenge.FailEffects.Add(DropHints);
                await Task.CompletedTask;

                /*******************************************************************/
                async Task DropHints() =>
                                    await _gameActionsProvider.Create<DropKeyGameAction>().SetWith(_gameActionsProvider.CurrentChallenge.ActiveInvestigator,
                    _gameActionsProvider.CurrentChallenge.ActiveInvestigator.CurrentPlace.Hints,
                    amount: _gameActionsProvider.CurrentChallenge.ActiveInvestigator.Hints.Value).Execute();
            }
        }
    }
}
