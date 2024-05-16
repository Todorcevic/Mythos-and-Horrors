using MythosAndHorrors.GameRules;
using Zenject;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.EditMode.Tests
{
    public abstract class Preparation
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public abstract CardPlace StartingPlace { get; }

        /*******************************************************************/
        public abstract Task PlaceAllScene();

        public async Task PlayThisInvestigator(Investigator investigator, bool withCards = true, bool withResources = true, bool withAvatar = true)
        {
            await _gameActionsProvider.Create(new MoveCardsGameAction(GetCardZonesInvestigator(investigator, withCards)));
            if (withResources)
                await _gameActionsProvider.Create(new GainResourceGameAction(investigator, 5));
            if (withAvatar)
                await _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, StartingPlace));
        }

        public async Task PlayAllInvestigators(bool withCards = true, bool withResources = true, bool withAvatar = true)
        {
            foreach (Investigator investigator in _investigatorsProvider.AllInvestigators)
            {
                if (withCards)
                    await _gameActionsProvider.Create(new MoveCardsGameAction(GetCardZonesInvestigator(investigator, true)));
                if (withResources)
                    await _gameActionsProvider.Create(new GainResourceGameAction(investigator, 5));
            }
            if (withAvatar)
                await _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.AllInvestigators, StartingPlace));
        }

        public async Task WasteTurnsInvestigator(Investigator investigator)
        {
            await _gameActionsProvider.Create(new UpdateStatGameAction(investigator.CurrentTurns, 0));
        }

        public async Task WasteAllTurns()
        {
            await _gameActionsProvider.Create(new UpdateStatGameAction(_investigatorsProvider.AllInvestigatorsInPlay
                                .ToDictionary(investigator => investigator.CurrentTurns, investigator => 0)));
        }

        public async Task StartingScene(bool withResources = false, bool withAvatar = true)
        {
            await PlaceAllScene();
            await PlayAllInvestigators(withResources: withResources, withAvatar: withAvatar);
        }

        private Dictionary<Card, (Zone zone, bool faceDown)> GetCardZonesInvestigator(Investigator investigator, bool withCards)
        {
            Dictionary<Card, (Zone zone, bool faceDown)> moveInvestigatorCards = new()
            {
                { investigator.InvestigatorCard, (investigator.InvestigatorZone, false) }
            };

            if (withCards)
            {
                investigator.FullDeck.Take(5).ForEach(card => moveInvestigatorCards.Add(card, (investigator.HandZone, false)));
                investigator.FullDeck.Skip(5).ForEach(card => moveInvestigatorCards.Add(card, (investigator.DeckZone, true)));
            }
            return moveInvestigatorCards;
        }
    }
}
