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
        public List<Card> Secta => new() { Drew, Herman, Peter, Victoria, Ruth };
        public CardPlace Home => _cardsProvider.GetCard<Card01124>();
        public CardPlace Fluvial => _cardsProvider.GetCard<Card01125>();
        public CardPlace Hospital => _cardsProvider.GetCard<Card01128>();
        public CardPlace University => _cardsProvider.GetCard<Card01129>();
        public CardPlace West => _cardsProvider.GetCard<Card01132>();
        public CardPlace Graveyard => _cardsProvider.GetCard<Card01133>();
        public CardPlace North => _cardsProvider.GetCard<Card01134>();
        public CardPlace South { get; private set; }
        public CardPlace Center { get; private set; }

        public IEnumerable<Card> StartDangerCards => _chaptersProvider.CurrentChapter.IsRegistered(CORERegister.PriestGhoulLive) ?
                     Info.DangerCards.Except(Secta) :
                     Info.DangerCards.Except(Secta).Except(new[] { GhoulPriest });

        public IEnumerable<CardCreature> Acolits => _cardsProvider.AllCards.OfType<Card01169>();

        /*******************************************************************/
        public override async Task PrepareScene()
        {
            SelectRandoms();
            await ShowHistory();
            await PlacePlaces();
            await PlaceDangerDeck();
            await PlaceAcolits();
            await PlacePlotAndGoal();
            await PlaceInvestigators();
        }

        /*******************************************************************/
        private void SelectRandoms()
        {
            South = new List<CardPlace> { _cardsProvider.GetCard<Card01126>(), _cardsProvider.GetCard<Card01127>() }.Rand();
            Center = new List<CardPlace> { _cardsProvider.GetCard<Card01130>(), _cardsProvider.GetCard<Card01131>() }.Rand();
        }

        private async Task ShowHistory()
        {
            if (_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.LitaGoAway))
                await _gameActionsProvider.Create(new ShowHistoryGameAction(Info.Descriptions[0]));
            else await _gameActionsProvider.Create(new ShowHistoryGameAction(Info.Descriptions[1]));
            await _gameActionsProvider.Create(new ShowHistoryGameAction(Info.Descriptions[2]));
        }

        private async Task PlacePlaces()
        {
            Dictionary<Card, Zone> allPlaces = new()
            {
                { South, PlaceZone[0, 3] },
                { Hospital, PlaceZone[0, 2] },
                { Graveyard, PlaceZone[1, 4] },
                { Fluvial, PlaceZone[1, 3] },
                { University, PlaceZone[1, 2] },
                { West, PlaceZone[2, 4] },
                { Center, PlaceZone[2, 3] },
                { North, PlaceZone[2, 2] }
            };


            if (_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.HouseUp))
                allPlaces.Add(Home, PlaceZone[0, 4]);

            await _gameActionsProvider.Create(new MoveCardsGameAction(allPlaces));
        }

        private async Task PlaceAcolits()
        {
            if (_investigatorsProvider.AllInvestigatorsInPlay.Count() > 1)
                await _gameActionsProvider.Create(new MoveCardsGameAction(Acolits.ElementAt(0), South.OwnZone));
            if (_investigatorsProvider.AllInvestigatorsInPlay.Count() > 2)
                await _gameActionsProvider.Create(new MoveCardsGameAction(Acolits.ElementAt(1), Center.OwnZone));
            if (_investigatorsProvider.AllInvestigatorsInPlay.Count() > 3)
                await _gameActionsProvider.Create(new MoveCardsGameAction(Acolits.ElementAt(2), Graveyard.OwnZone));
        }

        private async Task PlaceDangerDeck()
        {
            await _gameActionsProvider.Create(new MoveCardsGameAction(StartDangerCards, DangerDeckZone, isFaceDown: true));
            await _gameActionsProvider.Create(new ShuffleGameAction(DangerDeckZone));
        }

        private async Task PlacePlotAndGoal()
        {
            await _gameActionsProvider.Create(new PlacePlotGameAction(FirstPlot));
            await _gameActionsProvider.Create(new PlaceGoalGameAction(FirstGoal));
        }

        private async Task PlaceInvestigators()
        {
            if (_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.HouseUp))
                await _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.AllInvestigatorsInPlay, Home));
            else
                await _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.AllInvestigatorsInPlay, Fluvial));
        }

        /*******************************************************************/
        protected override async Task Resolution0()
        {
            await Interrogates();
            await _gameActionsProvider.Create(new GainSceneXpGameAction());

        }

        protected override async Task Resolution1()
        {
            await Interrogates();
            await _gameActionsProvider.Create(new RegisterChapterGameAction(CORERegister.IsMidknigh, true));
            await _gameActionsProvider.Create(new GainSceneXpGameAction());
        }

        private async Task Interrogates()
        {
            if (Drew.CurrentZone == VictoryZone)
                await _gameActionsProvider.Create(new RegisterChapterGameAction(CORERegister.DrewInterrogate, true));
            if (Herman.CurrentZone == VictoryZone)
                await _gameActionsProvider.Create(new RegisterChapterGameAction(CORERegister.HermanInterrogate, true));
            if (Peter.CurrentZone == VictoryZone)
                await _gameActionsProvider.Create(new RegisterChapterGameAction(CORERegister.PeterInterrogate, true));
            if (Victoria.CurrentZone == VictoryZone)
                await _gameActionsProvider.Create(new RegisterChapterGameAction(CORERegister.VictoriaInterrogate, true));
            if (Ruth.CurrentZone == VictoryZone)
                await _gameActionsProvider.Create(new RegisterChapterGameAction(CORERegister.RuthInterrogate, true));
            if (MaskedHunter.CurrentZone == VictoryZone)
                await _gameActionsProvider.Create(new RegisterChapterGameAction(CORERegister.MaskedHunterInterrogate, true));
            if (GhoulPriest.CurrentZone == VictoryZone)
                await _gameActionsProvider.Create(new RegisterChapterGameAction(CORERegister.PriestGhoulLive, false));
        }

        /*******************************************************************/
        protected override void PrepareChallengeTokens()
        {
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
            else return CreatureHardValue();

            /*******************************************************************/
            int CreatureNormalValue() => _cardsProvider.GetCards<CardCreature>()
                .Where(creature => creature.IsInPlay && creature.HasThisTag(Tag.Cultist))
                .OfType<IEldritchable>().Select(eldritchable => eldritchable.Eldritch.Value)
                .OrderByDescending(eldritch => eldritch).FirstOrDefault();
            int CreatureHardValue() => CurrentPlot.Eldritch.Value - (CurrentPlot.Info.Eldritch ?? 0);
        }

        private async Task CreatureEffect()
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                await CreatureNormalEffect();
            else await CreatureHardEffect();

            /*******************************************************************/
            async Task CreatureNormalEffect() => await Task.CompletedTask;
            async Task CreatureHardEffect() => await Task.CompletedTask;
        }

        private int CultistValue()
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                return CultistNormalValue();
            else return CultistHardValue();

            /*******************************************************************/
            int CultistNormalValue() => -2;
            int CultistHardValue() => -2;
        }

        private async Task CultistEffect()
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                await CultistNormalEffect();
            else await CultistHardEffect();

            /*******************************************************************/
            async Task CultistNormalEffect()
            {
                IEldritchable nearestCreature = _gameActionsProvider.CurrentChallenge.ActiveInvestigator.NearestCreatures
                    .Where(creature => creature.HasThisTag(Tag.Cultist)).OfType<IEldritchable>().FirstOrDefault();

                await _gameActionsProvider.Create(new IncrementStatGameAction(nearestCreature.Eldritch, 1));
            }

            async Task CultistHardEffect()
            {
                Dictionary<Stat, int> allEldrichableStats = _cardsProvider.GetCards<CardCreature>()
                    .Where(creature => creature.IsInPlay && creature.HasThisTag(Tag.Cultist))
                        .OfType<IEldritchable>().ToDictionary(cultist => cultist.Eldritch, cultist => 1);

                await _gameActionsProvider.Create(new IncrementStatGameAction(allEldrichableStats));
            }
        }

        private int DangerValue()
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                return DangerNormalValue();
            else return DangerHardValue();

            /*******************************************************************/
            int DangerNormalValue() => -3;
            int DangerHardValue() => -4;
        }

        private async Task DangerEffect()
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
                await _gameActionsProvider.Create(new DropHintGameAction(
                    _gameActionsProvider.CurrentChallenge.ActiveInvestigator,
                    _gameActionsProvider.CurrentChallenge.ActiveInvestigator.CurrentPlace.Hints, amount: 1));
            }

            async Task DangerHardEffect()
            {
                _gameActionsProvider.CurrentChallenge.FailEffects.Add(DropHints);
                await Task.CompletedTask;

                /*******************************************************************/
                async Task DropHints() =>
                await _gameActionsProvider.Create(new DropHintGameAction(
                    _gameActionsProvider.CurrentChallenge.ActiveInvestigator,
                    _gameActionsProvider.CurrentChallenge.ActiveInvestigator.CurrentPlace.Hints,
                    amount: _gameActionsProvider.CurrentChallenge.ActiveInvestigator.Hints.Value));
            }
        }
    }
}
