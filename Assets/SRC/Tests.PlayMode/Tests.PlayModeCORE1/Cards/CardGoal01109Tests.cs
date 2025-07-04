﻿using MythosAndHorrors.GameRules;
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
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.Hallway, _chaptersProvider.CurrentScene.GetPlaceZone(0, 3)).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.Parlor, _chaptersProvider.CurrentScene.GetPlaceZone(1, 3)).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.AllInvestigatorsInPlay, SceneCORE1.Hallway).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardGoal, _chaptersProvider.CurrentScene.GoalZone).Execute().AsCoroutine();

            yield return _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(cardGoal.Keys, cardGoal.Keys.Value).Execute().AsCoroutine();

            Assert.That(SceneCORE1.Parlor.Revealed.IsActive, Is.True);
            Assert.That(cardGoal.IsComplete, Is.True);
            Assert.That(SceneCORE1.Lita.CurrentZone, Is.EqualTo(SceneCORE1.Parlor.OwnZone));
            Assert.That(SceneCORE1.GhoulPriest.CurrentPlace, Is.EqualTo(SceneCORE1.Hallway));
        }

        [UnityTest]
        public IEnumerator PayKey()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01109>();
            yield return StartingScene();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.Second, SceneCORE1.Hallway).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.Leader, SceneCORE1.Hallway).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.CurrentGoal, _chaptersProvider.CurrentScene.OutZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardGoal, _chaptersProvider.CurrentScene.GoalZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(SceneCORE1.Hallway.Keys, cardGoal.Keys.Value).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<GainKeyGameAction>().SetWith(_investigatorsProvider.Leader, SceneCORE1.Hallway.Keys, 9).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<GainKeyGameAction>().SetWith(_investigatorsProvider.Second, SceneCORE1.Hallway.Keys, 3).Execute().AsCoroutine();
            yield return WasteAllTurns();

            Task taskGameAction = _gameActionsProvider.Create<RoundGameAction>().Execute();
            yield return ClickedIn(cardGoal);
            yield return ClickedIn(_investigatorsProvider.Leader.AvatarCard);
            yield return ClickedIn(_investigatorsProvider.Second.AvatarCard);
            yield return taskGameAction.AsCoroutine();

            Assert.That(_investigatorsProvider.Leader.Keys.Value, Is.EqualTo(0));
            Assert.That(_investigatorsProvider.Second.Keys.Value, Is.EqualTo(0));
            Assert.That(cardGoal.IsComplete, Is.True);
        }

        //[UnityTest]
        //public IEnumerator CancelPayKey()
        //{
        //    CardGoal cardGoal = _cardsProvider.GetCard<Card01109>();
        //    yield return StartingScene();
        //    yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.Second, SceneCORE1.Hallway).Execute().AsCoroutine();
        //    yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.Leader, SceneCORE1.Hallway).Execute().AsCoroutine();
        //    yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.Third, SceneCORE1.Hallway).Execute().AsCoroutine();
        //    yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.CurrentGoal, _chaptersProvider.CurrentScene.OutZone).Execute().AsCoroutine();
        //    yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardGoal, _chaptersProvider.CurrentScene.GoalZone).Execute().AsCoroutine();
        //    yield return _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(SceneCORE1.Hallway.Keys, cardGoal.Keys.Value).Execute().AsCoroutine();
        //    yield return _gameActionsProvider.Create<GainKeyGameAction>().SetWith(_investigatorsProvider.Leader, SceneCORE1.Hallway.Keys, 8).Execute().AsCoroutine();
        //    yield return _gameActionsProvider.Create<GainKeyGameAction>().SetWith(_investigatorsProvider.Second, SceneCORE1.Hallway.Keys, 3).Execute().AsCoroutine();
        //    yield return _gameActionsProvider.Create<GainKeyGameAction>().SetWith(_investigatorsProvider.Third, SceneCORE1.Hallway.Keys, 1).Execute().AsCoroutine();

        //    yield return WasteAllTurns();

        //    Task taskGameAction = _gameActionsProvider.Create<RoundGameAction>().Execute();
        //    yield return ClickedIn(cardGoal);
        //    yield return ClickedIn(_investigatorsProvider.Leader.AvatarCard);
        //    yield return ClickedUndoButton();
        //    yield return ClickedMainButton();

        //    yield return taskGameAction.AsCoroutine();
        //    Assert.That(_investigatorsProvider.Leader.Keys.Value, Is.EqualTo(8));
        //    Assert.That(_investigatorsProvider.Second.Keys.Value, Is.EqualTo(3));
        //    Assert.That(cardGoal.Keys.Value, Is.EqualTo(12));
        //    Assert.That(cardGoal.Revealed.IsActive, Is.False);
        //}
    }
}
