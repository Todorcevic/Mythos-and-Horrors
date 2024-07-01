using MythosAndHorrors.GameRules;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.PlayMode.Tests
{
    public abstract class Preparation : TestFixtureBase
    {
        public abstract CardPlace StartingPlace { get; }

        /*******************************************************************/
        public IEnumerator PlaceOnlyScene()
        {
            yield return PlaceScene().AsCoroutine().Fast();
        }

        public IEnumerator PlayThisInvestigator(Investigator investigator, bool withCards = true, bool withResources = true, bool withAvatar = true)
        {
            yield return PlayInvestigator(investigator, withCards, withResources, withAvatar).AsCoroutine().Fast();
        }

        public IEnumerator PlayAllInvestigators(bool withCards = true, bool withResources = false, bool withAvatar = true)
        {
            yield return PlayAllInvestigatorsAsync(withCards, withResources, withAvatar).AsCoroutine().Fast();
        }

        public IEnumerator WasteTurnsInvestigator(Investigator investigator)
        {
            yield return WasteTurns(investigator).AsCoroutine();
        }

        public IEnumerator WasteAllTurns()
        {
            yield return WasteAll().AsCoroutine();
        }

        public IEnumerator StartingScene(bool withResources = false, bool withAvatar = true)
        {
            yield return PlaceOnlyScene();
            yield return PlayAllInvestigators(withResources: withResources, withAvatar: withAvatar);
        }

        /*******************************************************************/
        protected abstract Task PlaceScene();

        private async Task PlayInvestigator(Investigator investigator, bool withCards = true, bool withResources = true, bool withAvatar = true)
        {
            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(GetCardZonesInvestigator(investigator, withCards)).Start();
            if (withResources)
                await _gameActionsProvider.Create(new GainResourceGameAction(investigator, 5));
            if (withAvatar)
                await _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, StartingPlace).Start();
        }

        private async Task PlayAllInvestigatorsAsync(bool withCards = true, bool withResources = true, bool withAvatar = true)
        {
            foreach (Investigator investigator in _investigatorsProvider.AllInvestigators)
            {
                if (withCards)
                    await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(GetCardZonesInvestigator(investigator, true)).Start();
                if (withResources)
                    await _gameActionsProvider.Create(new GainResourceGameAction(investigator, 5));
            }
            if (withAvatar)
                await _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.AllInvestigators, StartingPlace).Start();
        }

        private async Task WasteTurns(Investigator investigator)
        {
            await _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(investigator.CurrentTurns, 0).Start();
        }

        private async Task WasteAll()
        {
            await _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(_investigatorsProvider.AllInvestigatorsInPlay.ToDictionary(investigator => investigator.CurrentTurns, investigator => 0)).Start();
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
