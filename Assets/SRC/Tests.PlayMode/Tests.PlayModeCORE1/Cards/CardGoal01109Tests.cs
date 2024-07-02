using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardGoal01109Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator Reveal()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01109>();
            yield return PlayAllInvestigators();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.Hallway, _chaptersProvider.CurrentScene.GetPlaceZone(0, 3)).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.Parlor, _chaptersProvider.CurrentScene.GetPlaceZone(1, 3)).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.AllInvestigatorsInPlay, SceneCORE1.Hallway).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardGoal, _chaptersProvider.CurrentScene.GoalZone).Start().AsCoroutine();

            yield return _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(cardGoal.Hints, cardGoal.Hints.Value).Start().AsCoroutine();

            Assert.That(SceneCORE1.Parlor.Revealed.IsActive, Is.True);
            Assert.That(cardGoal.IsComplete, Is.True);
            Assert.That(SceneCORE1.Lita.CurrentZone, Is.EqualTo(SceneCORE1.Parlor.OwnZone));
            Assert.That(SceneCORE1.GhoulPriest.CurrentPlace, Is.EqualTo(SceneCORE1.Hallway));
        }

        [UnityTest]
        public IEnumerator PayHint()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01109>();
            yield return StartingScene();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.Second, SceneCORE1.Hallway).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.Leader, SceneCORE1.Hallway).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.CurrentGoal, _chaptersProvider.CurrentScene.OutZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardGoal, _chaptersProvider.CurrentScene.GoalZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(SceneCORE1.Hallway.Hints, cardGoal.Hints.Value).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<GainHintGameAction>().SetWith(_investigatorsProvider.Leader, SceneCORE1.Hallway.Hints, cardGoal.Hints.Value - 3).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<GainHintGameAction>().SetWith(_investigatorsProvider.Second, SceneCORE1.Hallway.Hints, 3).Start().AsCoroutine();
            yield return WasteAllTurns();

            Task taskGameAction = _gameActionsProvider.Create(new RoundGameAction());
            yield return ClickedIn(cardGoal);
            yield return ClickedIn(_investigatorsProvider.Leader.AvatarCard);
            yield return ClickedIn(_investigatorsProvider.Second.AvatarCard);
            yield return taskGameAction.AsCoroutine();

            Assert.That(_investigatorsProvider.Leader.Hints.Value, Is.EqualTo(0));
            Assert.That(_investigatorsProvider.Second.Hints.Value, Is.EqualTo(0));
            Assert.That(cardGoal.IsComplete, Is.True);
        }

        [UnityTest]
        public IEnumerator CancelPayHint()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01109>();
            yield return StartingScene();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.Second, SceneCORE1.Hallway).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.Leader, SceneCORE1.Hallway).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.Third, SceneCORE1.Hallway).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.CurrentGoal, _chaptersProvider.CurrentScene.OutZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardGoal, _chaptersProvider.CurrentScene.GoalZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(SceneCORE1.Hallway.Hints, cardGoal.Hints.Value).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<GainHintGameAction>().SetWith(_investigatorsProvider.Leader, SceneCORE1.Hallway.Hints, 8).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<GainHintGameAction>().SetWith(_investigatorsProvider.Second, SceneCORE1.Hallway.Hints, 3).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<GainHintGameAction>().SetWith(_investigatorsProvider.Third, SceneCORE1.Hallway.Hints, 1).Start().AsCoroutine();

            yield return WasteAllTurns();

            Task taskGameAction = _gameActionsProvider.Create(new RoundGameAction());
            yield return ClickedIn(cardGoal);
            yield return ClickedIn(_investigatorsProvider.Leader.AvatarCard);
            yield return ClickedUndoButton();
            yield return ClickedMainButton();

            yield return taskGameAction.AsCoroutine();
            Assert.That(_investigatorsProvider.Leader.Hints.Value, Is.EqualTo(8));
            Assert.That(_investigatorsProvider.Second.Hints.Value, Is.EqualTo(3));
            Assert.That(cardGoal.Hints.Value, Is.EqualTo(12));
            Assert.That(cardGoal.Revealed.IsActive, Is.False);
        }
    }
}
