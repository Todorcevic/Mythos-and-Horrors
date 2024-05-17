using MythosAndHorrors.GameRules;
using System.Linq;
using UnityEngine.TestTools;
using UnityEngine;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class SafeForeachTest : TestCORE1PlayModeBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator SafeForeach()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01108>();
            yield return _preparationSceneCORE1.StartingScene();

            IEnumerable<Investigator> AllInvestigators() => _investigatorsProvider.AllInvestigatorsInPlay
                .Where(investigator => investigator.CurrentPlace == _preparationSceneCORE1.SceneCORE1.Study);

            yield return _gameActionsProvider.Create(new RevealGameAction(_preparationSceneCORE1.SceneCORE1.Hallway)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.First, _preparationSceneCORE1.SceneCORE1.Hallway)).AsCoroutine();
            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.First));
            if (!DEBUG_MODE) yield return WaitToTokenClick();
            if (!DEBUG_MODE) yield return WaitToTokenClick();
            if (!DEBUG_MODE) yield return WaitToTokenClick();
            yield return gameActionTask.AsCoroutine();

            yield return _gameActionsProvider.Create(new SafeForeach<Investigator>(AllInvestigators, Discard)).AsCoroutine();


            if (DEBUG_MODE) yield return PressAnyKey();
            Assert.That(_investigatorsProvider.First.HandSize, Is.EqualTo(4));
        }

        [UnityTest]
        public IEnumerator SafeForeach2()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01108>();
            yield return _preparationSceneCORE1.StartingScene();

            IEnumerable<Investigator> AllInvestigators() => _investigatorsProvider.AllInvestigatorsInPlay
                .Where(investigator => investigator.CurrentPlace == _preparationSceneCORE1.SceneCORE1.Study);

            yield return _gameActionsProvider.Create(new RevealGameAction(_preparationSceneCORE1.SceneCORE1.Hallway)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.First, _preparationSceneCORE1.SceneCORE1.Hallway)).AsCoroutine();
            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.First));
            if (!DEBUG_MODE) yield return WaitToTokenClick();
            if (!DEBUG_MODE) yield return WaitToTokenClick();
            if (!DEBUG_MODE) yield return WaitToTokenClick();
            yield return gameActionTask.AsCoroutine();

            gameActionTask = _gameActionsProvider.Create(new SafeForeach<Investigator>(AllInvestigators, DiscardSelection));
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.Second.HandZone.Cards.First());
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.First.HandZone.Cards.First());
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.Third.HandZone.Cards.First());
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.Fourth.HandZone.Cards.First());

            yield return gameActionTask.AsCoroutine();
            if (DEBUG_MODE) yield return PressAnyKey();
            Assert.That(_investigatorsProvider.First.HandSize, Is.EqualTo(4));
        }

        [UnityTest]
        public IEnumerator SafeForeachUndo()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01108>();
            yield return _preparationSceneCORE1.StartingScene();

            IEnumerable<Investigator> AllInvestigators() => _investigatorsProvider.AllInvestigatorsInPlay
                .Where(investigator => investigator.CurrentPlace == _preparationSceneCORE1.SceneCORE1.Study);

            yield return _gameActionsProvider.Create(new RevealGameAction(_preparationSceneCORE1.SceneCORE1.Hallway)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.First, _preparationSceneCORE1.SceneCORE1.Hallway)).AsCoroutine();
            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.First));
            if (!DEBUG_MODE) yield return WaitToTokenClick();
            if (!DEBUG_MODE) yield return WaitToTokenClick();
            if (!DEBUG_MODE) yield return WaitToTokenClick();
            yield return gameActionTask.AsCoroutine();

            gameActionTask = _gameActionsProvider.Create(new SafeForeach<Investigator>(AllInvestigators, DiscardSelection));
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.Second.HandZone.Cards.First());
            if (!DEBUG_MODE) yield return WaitToUndoClick();
            if (!DEBUG_MODE) yield return WaitToTokenClick();

            yield return gameActionTask.AsCoroutine();
            if (DEBUG_MODE) yield return PressAnyKey();
            Assert.That(_investigatorsProvider.First.HandSize, Is.EqualTo(5));
        }

        private async Task Discard(Investigator investigator)
        {
            await _gameActionsProvider.Create(new DiscardGameAction(investigator.HandZone.Cards.First()));
            await _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.First, _preparationSceneCORE1.SceneCORE1.Study));
        }

        private async Task DiscardSelection(Investigator investigator)
        {
            InteractableGameAction interactableGameAction = new(canBackToThisInteractable: false, mustShowInCenter: true, "Select Tome", investigator);

            foreach (Card card in investigator.HandZone.Cards)
            {
                interactableGameAction.Create()
                    .SetCard(card)
                    .SetInvestigator(investigator)
                    .SetLogic(Activate);

                /*******************************************************************/
                async Task Activate()
                {
                    await _gameActionsProvider.Create(new DiscardGameAction(card));
                    await _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.First, _preparationSceneCORE1.SceneCORE1.Study));
                }

            }
            await _gameActionsProvider.Create(interactableGameAction);
        }
    }
}
