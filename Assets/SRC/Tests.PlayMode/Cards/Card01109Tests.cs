using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class Card01109Tests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator RevealTest()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01109>();
            yield return _preparationScene.PlayAllInvestigators();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationScene.SceneCORE1.Hallway, _chaptersProvider.CurrentScene.PlaceZone[0, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationScene.SceneCORE1.Parlor, _chaptersProvider.CurrentScene.PlaceZone[1, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.AllInvestigatorsInPlay, _preparationScene.SceneCORE1.Hallway)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardGoal, _chaptersProvider.CurrentScene.GoalZone)).AsCoroutine();

            yield return _gameActionsProvider.Create(new DecrementStatGameAction(cardGoal.Hints, cardGoal.Hints.Value)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_preparationScene.SceneCORE1.Parlor.Revealed.IsActive, Is.True);
            Assert.That(cardGoal.Revealed.IsActive, Is.True);
            Assert.That(_preparationScene.SceneCORE1.Lita.CurrentZone, Is.EqualTo(_preparationScene.SceneCORE1.Parlor.OwnZone));
            Assert.That(_preparationScene.SceneCORE1.GhoulPriest.CurrentPlace, Is.EqualTo(_preparationScene.SceneCORE1.Hallway));
        }

        [UnityTest]
        public IEnumerator PayHintTest()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01109>();
            yield return _preparationScene.StartingScene();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.Second, _preparationScene.SceneCORE1.Hallway)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.Leader, _preparationScene.SceneCORE1.Hallway)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationScene.SceneCORE1.CurrentGoal, _chaptersProvider.CurrentScene.OutZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardGoal, _chaptersProvider.CurrentScene.GoalZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new IncrementStatGameAction(_preparationScene.SceneCORE1.Hallway.Hints, cardGoal.Hints.Value)).AsCoroutine();
            yield return _gameActionsProvider.Create(new GainHintGameAction(_investigatorsProvider.Leader, _preparationScene.SceneCORE1.Hallway.Hints, cardGoal.Hints.Value - 3)).AsCoroutine();
            yield return _gameActionsProvider.Create(new GainHintGameAction(_investigatorsProvider.Second, _preparationScene.SceneCORE1.Hallway.Hints, 3)).AsCoroutine();
            yield return _preparationScene.WasteAllTurns();

            Task taskGameAction = _gameActionsProvider.Create(new RoundGameAction());
            if (!DEBUG_MODE) yield return WaitToClick(cardGoal);
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.Leader.AvatarCard);
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.Second.AvatarCard);

            while (!taskGameAction.IsCompleted) yield return null;
            Assert.That(_investigatorsProvider.Leader.Hints.Value, Is.EqualTo(0));
            Assert.That(_investigatorsProvider.Second.Hints.Value, Is.EqualTo(0));
            Assert.That(cardGoal.Hints.Value, Is.EqualTo(0));
            Assert.That(cardGoal.Revealed.IsActive, Is.True);
        }

        [UnityTest]
        public IEnumerator CancelPayHintTest()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01109>();
            yield return _preparationScene.StartingScene();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.Second, _preparationScene.SceneCORE1.Hallway)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.Leader, _preparationScene.SceneCORE1.Hallway)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationScene.SceneCORE1.CurrentGoal, _chaptersProvider.CurrentScene.OutZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardGoal, _chaptersProvider.CurrentScene.GoalZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new IncrementStatGameAction(_preparationScene.SceneCORE1.Hallway.Hints, cardGoal.Hints.Value)).AsCoroutine();
            yield return _gameActionsProvider.Create(new GainHintGameAction(_investigatorsProvider.Leader, _preparationScene.SceneCORE1.Hallway.Hints, cardGoal.Hints.Value - 3)).AsCoroutine();
            yield return _gameActionsProvider.Create(new GainHintGameAction(_investigatorsProvider.Second, _preparationScene.SceneCORE1.Hallway.Hints, 3)).AsCoroutine();
            yield return _preparationScene.WasteAllTurns();

            Task taskGameAction = _gameActionsProvider.Create(new RoundGameAction());
            if (!DEBUG_MODE) yield return WaitToClick(cardGoal);
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.Leader.AvatarCard);
            Assert.That(_mainButtonComponent.GetPrivateMember<bool>("IsActivated"), Is.False);

            if (!DEBUG_MODE) yield return WaitToUndoClick();
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (!taskGameAction.IsCompleted) yield return null;
            Assert.That(_investigatorsProvider.Leader.Hints.Value, Is.EqualTo(9));
            Assert.That(_investigatorsProvider.Second.Hints.Value, Is.EqualTo(3));
            Assert.That(cardGoal.Hints.Value, Is.EqualTo(12));
            Assert.That(cardGoal.Revealed.IsActive, Is.False);
        }
    }
}
