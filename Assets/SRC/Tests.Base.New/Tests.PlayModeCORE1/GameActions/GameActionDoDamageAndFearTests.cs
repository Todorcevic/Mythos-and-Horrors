﻿using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class GameActionDoDamageAndFearTests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator HarmTest()
        {
            Card bulletProof = BuilCard("01594");
            Investigator investigator = _investigatorsProvider.First;
            Card damageableCard = investigator.AllCards.First(card => card.Info.Code == "01521");
            Card damageableCard2 = investigator.AllCards.First(card => card.Info.Code == "01521" && card != damageableCard);
            yield return StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(damageableCard, investigator.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(damageableCard2, investigator.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(bulletProof, investigator.AidZone)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new HarmToInvestigatorGameAction(investigator, SceneCORE1.GhoulSecuaz, amountDamage: 2, amountFear: 1));

            yield return ClickedIn(bulletProof);
            yield return ClickedIn(damageableCard);
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.AidZone.Cards.Count, Is.EqualTo(2));
            Assert.That(((IDamageable)bulletProof).Health.Value, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator UndoHarmTest()
        {
            Card bulletProof = BuilCard("01594");
            Investigator investigator = _investigatorsProvider.First;
            Card damageableCard = investigator.AllCards.First(card => card.Info.Code == "01521");
            Card damageableCard2 = investigator.AllCards.First(card => card.Info.Code == "01521" && card != damageableCard);
            yield return StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(damageableCard, investigator.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(damageableCard2, investigator.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(bulletProof, investigator.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, SceneCORE1.Hallway)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(SceneCORE1.Attic);
            yield return ClickedIn(damageableCard);
            yield return ClickedUndoButton();
            yield return ClickedIn(investigator.InvestigatorCard);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.AidZone.Cards.Count, Is.EqualTo(3));
            Assert.That(investigator.Sanity.Value, Is.EqualTo(4));
        }
    }
}