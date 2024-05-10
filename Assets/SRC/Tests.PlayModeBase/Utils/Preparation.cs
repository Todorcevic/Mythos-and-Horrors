using MythosAndHorrors.GameRules;
using Zenject;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MythosAndHorrors.PlayMode.Tests
{
    public abstract class Preparation
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        protected abstract CardPlace StartingPlace { get; }

        /*******************************************************************/
        public abstract IEnumerator PlaceAllScene();

        public IEnumerator PlayThisInvestigator(Investigator investigator, bool withCards = true, bool withResources = true, bool withAvatar = true)
        {
            float currentTimeScale = Time.timeScale;
            Time.timeScale = 64;
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(GetCardZonesInvestigator(investigator, withCards))).AsCoroutine();
            if (withResources)
                yield return _gameActionsProvider.Create(new GainResourceGameAction(investigator, 5)).AsCoroutine();
            if (withAvatar)
                yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, StartingPlace)).AsCoroutine();
            Time.timeScale = currentTimeScale;
        }

        public IEnumerator PlayAllInvestigators(bool withCards = true, bool withResources = true, bool withAvatar = true)
        {
            float currentTimeScale = Time.timeScale;
            Time.timeScale = 64;
            foreach (Investigator investigator in _investigatorsProvider.AllInvestigators)
            {
                if (withCards)
                    yield return _gameActionsProvider.Create(new MoveCardsGameAction(GetCardZonesInvestigator(investigator, true))).AsCoroutine();
                if (withResources)
                    yield return _gameActionsProvider.Create(new GainResourceGameAction(investigator, 5)).AsCoroutine();
            }
            if (withAvatar)
                yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.AllInvestigators, StartingPlace)).AsCoroutine();
            Time.timeScale = currentTimeScale;
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

        public IEnumerator WasteTurnsInvestigator(Investigator investigator)
        {
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(investigator.CurrentTurns, 0)).AsCoroutine();
        }

        public IEnumerator WasteAllTurns()
        {
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(_investigatorsProvider.AllInvestigatorsInPlay
                               .ToDictionary(investigator => investigator.CurrentTurns, investigator => 0))).AsCoroutine();
        }

        public IEnumerator StartingScene()
        {
            yield return PlaceAllScene();
            yield return PlayAllInvestigators();
        }
    }
}
