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

        public IEnumerable<Card> Hastur => _cardsProvider.GetCards<Card01175>().Cast<Card>().Concat(_cardsProvider.GetCards<Card01176>());
        public IEnumerable<Card> Yog => _cardsProvider.GetCards<Card01177>().Cast<Card>().Concat(_cardsProvider.GetCards<Card01178>());
        public IEnumerable<Card> Shub => _cardsProvider.GetCards<Card01179>().Cast<Card>().Concat(_cardsProvider.GetCards<Card01180>());
        public IEnumerable<Card> Cthulhu => _cardsProvider.GetCards<Card01181>().Cast<Card>().Concat(_cardsProvider.GetCards<Card01182>());
        public IEnumerable<Card> AllAgents => Hastur.Concat(Yog).Concat(Shub).Concat(Cthulhu);
        public IEnumerable<Card> RealDangerCards => _chaptersProvider.CurrentChapter.IsRegistered(CORERegister.PriestGhoulLive) ?
            Info.DangerCards.Except(AllAgents.Except(AgentSelected)) :
            Info.DangerCards.Except(AllAgents.Except(AgentSelected)).Except(new[] { GhoulPriest });

        public int AmountInterrogate =>
            (_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.DrewInterrogate) ? 1 : 0) +
            (_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.HermanInterrogate) ? 1 : 0) +
            (_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.PeterInterrogate) ? 1 : 0) +
            (_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.RuthInterrogate) ? 1 : 0) +
            (_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.VictoriaInterrogate) ? 1 : 0) +
            (_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.MaskedHunterInterrogate) ? 1 : 0);

        public List<CardPlace> Forests => new() { Forest1, Forest2, Forest3, Forest4, Forest5, Forest6 };
        public IEnumerable<CardPlace> ForestsToPlace { get; private set; }
        public IEnumerable<Card> AgentSelected { get; private set; }

        /*******************************************************************/
        public override async Task PrepareScene()
        {
            SelectRandoms();
            await ShowHistory();
            await PlacePlaces();
            await PlaceDangerDeck();
            await PlacePlotAndGoal();
            await PlaceInvestigators();
            await CheckDiscard();
        }

        /*******************************************************************/
        private void SelectRandoms()
        {
            ForestsToPlace = Forests.Rand(4);
            AgentSelected = new List<IEnumerable<Card>> { Hastur, Yog, Shub, Cthulhu }.Rand();
        }

        private async Task ShowHistory()
        {
            await _gameActionsProvider.Create(new ShowHistoryGameAction(Info.Descriptions[0]));
        }

        private async Task PlacePlaces()
        {
            Dictionary<Card, Zone> allPlaces = new()
            {
                { MainPath, PlaceZone[1, 3] },
                { ForestsToPlace.ElementAt(0), PlaceZone[0, 2] },
                { ForestsToPlace.ElementAt(1), PlaceZone[0, 4] },
                { ForestsToPlace.ElementAt(2), PlaceZone[2, 2] },
                { ForestsToPlace.ElementAt(3), PlaceZone[2, 4] },
            };

            await _gameActionsProvider.Create(new MoveCardsGameAction(allPlaces));
        }

        private async Task PlaceDangerDeck()
        {
            await _gameActionsProvider.Create(new MoveCardsGameAction(RealDangerCards, DangerDeckZone, isFaceDown: true));
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
                await _gameActionsProvider.Create(new SafeForeach<Investigator>(InvestigatorsWithCards(), Discard));

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
            await _gameActionsProvider.Create(new SafeForeach<Investigator>(_investigatorsProvider.AllInvestigatorsInPlay, Defeated));

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
                await Task.CompletedTask; //TODO: Implement this method
            }
        }
    }
}
