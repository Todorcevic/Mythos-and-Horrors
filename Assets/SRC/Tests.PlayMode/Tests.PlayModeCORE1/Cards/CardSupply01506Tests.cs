﻿using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{

    public class CardSupply01506Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator Attack()
        {
            Investigator investigator = _investigatorsProvider.First;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_2);
            yield return StartingScene();
            Card01506 weaponCard = _cardsProvider.GetCard<Card01506>();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(weaponCard, investigator.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE1.GhoulSecuaz, investigator.DangerZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE1.GhoulVoraz, investigator.DangerZone)).AsCoroutine();

            Assert.That(weaponCard.AmountBullets.Value, Is.EqualTo(4));

            Task<PlayInvestigatorGameAction> taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(weaponCard);
            yield return ClickedIn(SceneCORE1.GhoulVoraz);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(SceneCORE1.GhoulVoraz.Health.Value, Is.EqualTo(1));
            Assert.That(weaponCard.AmountBullets.Value, Is.EqualTo(3));
        }

        [UnityTest]
        public IEnumerator NoBullets()
        {
            Investigator investigator = _investigatorsProvider.First;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_2);
            yield return StartingScene();
            Card01506 weaponCard = _cardsProvider.GetCard<Card01506>();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(weaponCard, investigator.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(weaponCard.AmountBullets, 0)).AsCoroutine();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE1.GhoulSecuaz, investigator.DangerZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE1.GhoulVoraz, investigator.DangerZone)).AsCoroutine();

            Task<PlayInvestigatorGameAction> taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));

            Assert.That(IsClickable(weaponCard), Is.False);

            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(weaponCard.AmountBullets.Value, Is.EqualTo(0));
        }
    }
}