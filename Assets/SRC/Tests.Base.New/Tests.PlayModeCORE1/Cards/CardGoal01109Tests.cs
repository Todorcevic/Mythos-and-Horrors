using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class CardGoal01109Tests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator Reveal()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01109>();
            yield return PlayAllInvestigators();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE1.Hallway, _chaptersProvider.CurrentScene.PlaceZone[0, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE1.Parlor, _chaptersProvider.CurrentScene.PlaceZone[1, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.AllInvestigatorsInPlay, SceneCORE1.Hallway)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardGoal, _chaptersProvider.CurrentScene.GoalZone)).AsCoroutine();

            yield return _gameActionsProvider.Create(new DecrementStatGameAction(cardGoal.Hints, cardGoal.Hints.Value)).AsCoroutine();

            Assert.That(SceneCORE1.Parlor.Revealed.IsActive, Is.True);
            Assert.That(cardGoal.Revealed.IsActive, Is.True);
            Assert.That(SceneCORE1.Lita.CurrentZone, Is.EqualTo(SceneCORE1.Parlor.OwnZone));
            Assert.That(SceneCORE1.GhoulPriest.CurrentPlace, Is.EqualTo(SceneCORE1.Hallway));
        }

        [UnityTest]
        public IEnumerator PayHint()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01109>();
            yield return StartingScene();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.Second, SceneCORE1.Hallway)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.Leader, SceneCORE1.Hallway)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE1.CurrentGoal, _chaptersProvider.CurrentScene.OutZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardGoal, _chaptersProvider.CurrentScene.GoalZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new IncrementStatGameAction(SceneCORE1.Hallway.Hints, cardGoal.Hints.Value)).AsCoroutine();
            yield return _gameActionsProvider.Create(new GainHintGameAction(_investigatorsProvider.Leader, SceneCORE1.Hallway.Hints, cardGoal.Hints.Value - 3)).AsCoroutine();
            yield return _gameActionsProvider.Create(new GainHintGameAction(_investigatorsProvider.Second, SceneCORE1.Hallway.Hints, 3)).AsCoroutine();
            yield return WasteAllTurns();

            Task taskGameAction = _gameActionsProvider.Create(new RoundGameAction());
            yield return ClickedIn(cardGoal);
            yield return ClickedIn(_investigatorsProvider.Leader.AvatarCard);
            yield return ClickedIn(_investigatorsProvider.Leader.AvatarCard);
            yield return ClickedIn(_investigatorsProvider.Leader.AvatarCard);
            yield return ClickedIn(_investigatorsProvider.Leader.AvatarCard);
            yield return ClickedIn(_investigatorsProvider.Leader.AvatarCard);
            yield return ClickedIn(_investigatorsProvider.Leader.AvatarCard);
            yield return ClickedIn(_investigatorsProvider.Leader.AvatarCard);
            yield return ClickedIn(_investigatorsProvider.Leader.AvatarCard);
            yield return ClickedIn(_investigatorsProvider.Leader.AvatarCard);
            yield return ClickedIn(_investigatorsProvider.Second.AvatarCard);
            yield return ClickedIn(_investigatorsProvider.Second.AvatarCard);
            yield return ClickedIn(_investigatorsProvider.Second.AvatarCard);
            yield return taskGameAction.AsCoroutine();

            Assert.That(_investigatorsProvider.Leader.Hints.Value, Is.EqualTo(0));
            Assert.That(_investigatorsProvider.Second.Hints.Value, Is.EqualTo(0));
            Assert.That(cardGoal.Hints.Value, Is.EqualTo(0));
            Assert.That(cardGoal.Revealed.IsActive, Is.True);
        }

        [UnityTest]
        public IEnumerator CancelPayHint()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01109>();
            yield return StartingScene();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.Second, SceneCORE1.Hallway)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.Leader, SceneCORE1.Hallway)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE1.CurrentGoal, _chaptersProvider.CurrentScene.OutZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardGoal, _chaptersProvider.CurrentScene.GoalZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new IncrementStatGameAction(SceneCORE1.Hallway.Hints, cardGoal.Hints.Value)).AsCoroutine();
            yield return _gameActionsProvider.Create(new GainHintGameAction(_investigatorsProvider.Leader, SceneCORE1.Hallway.Hints, cardGoal.Hints.Value - 3)).AsCoroutine();
            yield return _gameActionsProvider.Create(new GainHintGameAction(_investigatorsProvider.Second, SceneCORE1.Hallway.Hints, 3)).AsCoroutine();
            yield return WasteAllTurns();

            Task taskGameAction = _gameActionsProvider.Create(new RoundGameAction());
            yield return ClickedIn(cardGoal);
            yield return ClickedIn(_investigatorsProvider.Leader.AvatarCard);

            Assert.That(_gameActionsProvider.CurrentInteractable.MainButtonEffect, Is.Null);

            yield return ClickedUndoButton();
            yield return ClickedMainButton();

            yield return taskGameAction.AsCoroutine();
            Assert.That(_investigatorsProvider.Leader.Hints.Value, Is.EqualTo(9));
            Assert.That(_investigatorsProvider.Second.Hints.Value, Is.EqualTo(3));
            Assert.That(cardGoal.Hints.Value, Is.EqualTo(12));
            Assert.That(cardGoal.Revealed.IsActive, Is.False);
        }
    }
}
