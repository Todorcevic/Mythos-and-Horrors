﻿using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class Card01108Tests : TestBase
    {
        protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator RevealTest()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01108>();
            yield return _preparationScene.PlayAllInvestigators();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationScene.Study, _chaptersProvider.CurrentScene.PlaceZone[0, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.AllInvestigatorsInPlay, _preparationScene.Study));
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationScene.GhoulSecuaz, _preparationScene.Study.OwnZone)).AsCoroutine();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardGoal, _chaptersProvider.CurrentScene.GoalZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new DecrementStatGameAction(cardGoal.Hints, cardGoal.Hints.Value)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_preparationScene.Study.CurrentZone, Is.EqualTo(_chaptersProvider.CurrentScene.OutZone));
            Assert.That(_preparationScene.GhoulSecuaz.CurrentZone, Is.EqualTo(_chaptersProvider.CurrentScene.DangerDiscardZone));
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay
                .All(investigator => investigator.CurrentPlace == _preparationScene.Hallway), Is.True);
        }

        [UnityTest]
        public IEnumerator PayHintTest()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01108>();
            yield return _preparationScene.PlayAllInvestigators();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationScene.Study, _chaptersProvider.CurrentScene.PlaceZone[0, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.AllInvestigatorsInPlay, _preparationScene.Study)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationScene.GhoulSecuaz, _preparationScene.Study.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardGoal, _chaptersProvider.CurrentScene.GoalZone)).AsCoroutine();

            yield return _gameActionsProvider.Create(new IncrementStatGameAction(_preparationScene.Study.Hints, cardGoal.Hints.Value)).AsCoroutine();
            yield return _gameActionsProvider.Create(new GainHintGameAction(_investigatorsProvider.Leader, _preparationScene.Study.Hints, 5)).AsCoroutine();
            yield return _gameActionsProvider.Create(new GainHintGameAction(_investigatorsProvider.Second, _preparationScene.Study.Hints, 3)).AsCoroutine();

            yield return _gameActionsProvider.Create(new InvestigatorsPhaseGameAction()).AsCoroutine();
            //yield return _gameActionsProvider.Create(new DecrementStatGameAction(cardGoal.Hints, cardGoal.Hints.Value)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_preparationScene.Study.CurrentZone, Is.EqualTo(_chaptersProvider.CurrentScene.OutZone));
            Assert.That(_preparationScene.GhoulSecuaz.CurrentZone, Is.EqualTo(_chaptersProvider.CurrentScene.OutZone));
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay
                .All(investigator => investigator.CurrentPlace == _preparationScene.Hallway), Is.True);
        }
    }
}