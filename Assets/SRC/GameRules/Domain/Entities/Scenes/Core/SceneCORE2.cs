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

        public IEnumerable<Card> RealDangerCards => _chaptersProvider.CurrentChapter.IsRegistered(CORERegister.PriestGhoulLive) ?
                     Info.DangerCards.Except(Secta) :
                     Info.DangerCards.Except(Secta).Except(new[] { GhoulPriest });

        public IEnumerable<CardCreature> Acolits => _cardsProvider.AllCards.OfType<Card01169>();

        /*******************************************************************/
        public override async Task PrepareScene()
        {
            CreateReaction<EliminateInvestigatorGameAction>(InvestigatorsLooseCondition, InvestigatorsLooseLogic, isAtStart: false);
            SelectRandoms();
            await ShowHistory();
            await PlacePlaces();
            await PlaceAcolits();
            await PlaceDangerDeck();
            await PlacePlotAndGoal();
            await PlaceInvestigators();
        }

        /*******************************************************************/
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
            if (_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.HouseUp))
                await _gameActionsProvider.Create(new MoveCardsGameAction(Home, PlaceZone[0, 4]));
            await _gameActionsProvider.Create(new MoveCardsGameAction(South, PlaceZone[0, 3]));
            await _gameActionsProvider.Create(new MoveCardsGameAction(Hospital, PlaceZone[0, 2]));
            await _gameActionsProvider.Create(new MoveCardsGameAction(Graveyard, PlaceZone[1, 4]));
            await _gameActionsProvider.Create(new MoveCardsGameAction(Fluvial, PlaceZone[1, 3]));
            await _gameActionsProvider.Create(new MoveCardsGameAction(University, PlaceZone[1, 2]));
            await _gameActionsProvider.Create(new MoveCardsGameAction(West, PlaceZone[2, 4]));
            await _gameActionsProvider.Create(new MoveCardsGameAction(Center, PlaceZone[2, 3]));
            await _gameActionsProvider.Create(new MoveCardsGameAction(North, PlaceZone[2, 2]));
        }

        private async Task PlaceAcolits()
        {
            if (_investigatorsProvider.AllInvestigators.Count > 1)
                await _gameActionsProvider.Create(new MoveCardsGameAction(Acolits.ElementAt(0), South.OwnZone));
            if (_investigatorsProvider.AllInvestigators.Count > 2)
                await _gameActionsProvider.Create(new MoveCardsGameAction(Acolits.ElementAt(1), Center.OwnZone));
            if (_investigatorsProvider.AllInvestigators.Count > 3)
                await _gameActionsProvider.Create(new MoveCardsGameAction(Acolits.ElementAt(2), Graveyard.OwnZone));
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
        }

        private async Task PlaceInvestigators()
        {
            if (_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.HouseUp))
                await _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.AllInvestigatorsInPlay, Home));
            else
                await _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.AllInvestigatorsInPlay, Home));
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
    }

}
