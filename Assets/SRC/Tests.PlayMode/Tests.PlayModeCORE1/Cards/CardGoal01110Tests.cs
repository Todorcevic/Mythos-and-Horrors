﻿using MythosAndHorrors.GameRules;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardGoal01110Tests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator RevealNoBurn()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01110>();
            yield return PlayAllInvestigators();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE1.Hallway, _chaptersProvider.CurrentScene.PlaceZone[0, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE1.Parlor, _chaptersProvider.CurrentScene.PlaceZone[1, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.AllInvestigatorsInPlay, SceneCORE1.Hallway)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardGoal, _chaptersProvider.CurrentScene.GoalZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE1.GhoulPriest, SceneCORE1.Hallway.OwnZone)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new DefeatCardGameAction(SceneCORE1.GhoulPriest, _investigatorsProvider.First.InvestigatorCard));
            yield return ClickedClone(cardGoal, 1, isReaction: true);
            yield return taskGameAction.AsCoroutine();

            Assert.That(cardGoal.Revealed.IsActive, Is.True);
            Assert.That(_investigatorsProvider.First.Xp.Value, Is.EqualTo(5));
        }

        [UnityTest]
        public IEnumerator RevealBurnIt()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01110>();
            yield return PlayAllInvestigators();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE1.Hallway, _chaptersProvider.CurrentScene.PlaceZone[0, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE1.Parlor, _chaptersProvider.CurrentScene.PlaceZone[1, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.AllInvestigatorsInPlay, SceneCORE1.Hallway)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardGoal, _chaptersProvider.CurrentScene.GoalZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE1.GhoulPriest, SceneCORE1.Hallway.OwnZone)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new DefeatCardGameAction(SceneCORE1.GhoulPriest, _investigatorsProvider.First.InvestigatorCard));
            yield return ClickedClone(cardGoal, 0, isReaction: true);
            yield return taskGameAction.AsCoroutine();

            Assert.That(cardGoal.Revealed.IsActive, Is.True);
            Assert.That(_investigatorsProvider.First.Xp.Value, Is.EqualTo(4));
            Assert.That(_investigatorsProvider.First.Shock.Value, Is.EqualTo(1));
        }
    }
}