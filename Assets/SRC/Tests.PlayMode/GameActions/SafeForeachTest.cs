using MythosAndHorrors.GameRules;
using System.Linq;
using UnityEngine.TestTools;
using UnityEngine;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class SafeForeachTest : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator SafeForeach()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01108>();
            yield return _preparationScene.StartingScene();

            var allInvestigators = _investigatorsProvider.AllInvestigatorsInPlay
                .Where(i => i.CurrentPlace == _preparationScene.SceneCORE1.Study);

            yield return _gameActionsProvider.Create(new RevealGameAction(_preparationScene.SceneCORE1.Hallway)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.First, _preparationScene.SceneCORE1.Hallway)).AsCoroutine();
            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.First));
            if (!DEBUG_MODE) yield return WaitToTokenClick();
            if (!DEBUG_MODE) yield return WaitToTokenClick();
            if (!DEBUG_MODE) yield return WaitToTokenClick();
            while (!gameActionTask.IsCompleted) yield return null;

            yield return _gameActionsProvider.Create(new SafeForeach<Investigator>(allInvestigators, DrawDangerCard)).AsCoroutine();


            if (DEBUG_MODE) yield return PressAnyKey();
            Assert.That(_investigatorsProvider.First.HandSize, Is.EqualTo(4));
        }

        [UnityTest]
        public IEnumerator SafeForeach2()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01108>();
            yield return _preparationScene.StartingScene();

            var allInvestigators = _investigatorsProvider.AllInvestigatorsInPlay
                .Where(i => i.CurrentPlace == _preparationScene.SceneCORE1.Study);

            yield return _gameActionsProvider.Create(new RevealGameAction(_preparationScene.SceneCORE1.Hallway)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.First, _preparationScene.SceneCORE1.Hallway)).AsCoroutine();
            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.First));
            if (!DEBUG_MODE) yield return WaitToTokenClick();
            if (!DEBUG_MODE) yield return WaitToTokenClick();
            if (!DEBUG_MODE) yield return WaitToTokenClick();
            while (!gameActionTask.IsCompleted) yield return null;

            gameActionTask = _gameActionsProvider.Create(new SafeForeach<Investigator>(allInvestigators, DrawDangerCard2));
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.Second.HandZone.Cards.First());
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.First.HandZone.Cards.First());
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.Third.HandZone.Cards.First());
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.Fourth.HandZone.Cards.First());

            while (!gameActionTask.IsCompleted) yield return null;
            if (DEBUG_MODE) yield return PressAnyKey();
            Assert.That(_investigatorsProvider.First.HandSize, Is.EqualTo(4));
        }

        [UnityTest]
        public IEnumerator SafeForeach3()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01108>();
            yield return _preparationScene.StartingScene();

            var allInvestigators = _investigatorsProvider.AllInvestigatorsInPlay
                .Where(i => i.CurrentPlace == _preparationScene.SceneCORE1.Study);

            yield return _gameActionsProvider.Create(new RevealGameAction(_preparationScene.SceneCORE1.Hallway)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.First, _preparationScene.SceneCORE1.Hallway)).AsCoroutine();
            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.First));
            if (!DEBUG_MODE) yield return WaitToTokenClick();
            if (!DEBUG_MODE) yield return WaitToTokenClick();
            if (!DEBUG_MODE) yield return WaitToTokenClick();
            while (!gameActionTask.IsCompleted) yield return null;

            gameActionTask = _gameActionsProvider.Create(new SafeForeach<Investigator>(allInvestigators, DrawDangerCard2));
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.Second.HandZone.Cards.First());
            if (!DEBUG_MODE) yield return WaitToUndoClick();
            if (!DEBUG_MODE) yield return WaitToTokenClick();

            while (!gameActionTask.IsCompleted) yield return null;
            if (DEBUG_MODE) yield return PressAnyKey();
            Assert.That(_investigatorsProvider.First.HandSize, Is.EqualTo(5));
        }

        private async Task DrawDangerCard(Investigator investigator)
        {
            await _gameActionsProvider.Create(new DiscardGameAction(investigator.HandZone.Cards.First()));
            await _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.First, _preparationScene.SceneCORE1.Study));
        }

        private async Task DrawDangerCard2(Investigator investigator)
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
                    await _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.First, _preparationScene.SceneCORE1.Study));
                }

            }
            await _gameActionsProvider.Create(interactableGameAction);
        }
    }
}
