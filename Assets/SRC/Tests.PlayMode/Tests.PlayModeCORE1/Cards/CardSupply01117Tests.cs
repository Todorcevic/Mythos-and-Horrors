﻿using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01117Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator Attack()
        {
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value0);
            Investigator investigator = _investigatorsProvider.First;
            CardSupply Lita = _cardsProvider.GetCard<Card01117>();
            yield return StartingScene();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(Lita, investigator.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new SpawnCreatureGameAction(SceneCORE1.GhoulVoraz, SceneCORE1.Study)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedClone(SceneCORE1.GhoulVoraz, 0);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(SceneCORE1.GhoulVoraz.DamageRecived, Is.EqualTo(2));
            Assert.That(investigator.Strength.Value, Is.EqualTo(investigator.InvestigatorCard.Info.Strength + 1));
            Assert.That(_investigatorsProvider.Second.Strength.Value, Is.EqualTo(_investigatorsProvider.Second.InvestigatorCard.Info.Strength + 1));
        }

        [UnityTest]
        public IEnumerator Parlay()
        {
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value0);
            Investigator investigator = _investigatorsProvider.Second;
            CardSupply Lita = _cardsProvider.GetCard<Card01117>();
            yield return StartingScene();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(Lita, SceneCORE1.Study.OwnZone)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(Lita);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(Lita.CurrentPlace, Is.EqualTo(investigator.CurrentPlace));
            Assert.That(Lita.CurrentZone, Is.EqualTo(investigator.AidZone));
            Assert.That(investigator.Strength.Value, Is.EqualTo(investigator.InvestigatorCard.Info.Strength + 1));
            Assert.That(_investigatorsProvider.Second.Strength.Value, Is.EqualTo(_investigatorsProvider.Second.InvestigatorCard.Info.Strength + 1));


            //taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.First));
            //yield return ClickedTokenButton();
            //yield return _gameActionsProvider.Create(new MoveCardsGameAction(Lita, SceneCORE1.OutZone)).AsCoroutine();
            //yield return ClickedTokenButton();
            //yield return ClickedMainButton();
            //yield return taskGameAction.AsCoroutine();

            //Assert.That(investigator.Strength.Value, Is.EqualTo(investigator.InvestigatorCard.Info.Strength));

        }
        //protected override TestsType TestsType => TestsType.Debug;

    }
}