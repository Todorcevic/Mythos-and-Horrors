﻿using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardCreature01603Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator BlankCratureBuff()
        {
            Investigator investigator = _investigatorsProvider.First;
            Investigator investigator2 = _investigatorsProvider.Third;
            yield return StartingScene();
            CardCreature creature = _cardsProvider.GetCard<Card01603>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature, SceneCORE1.Study.OwnZone).Execute().AsCoroutine();
            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator2).Execute();
            while (_gameActionsProvider.CurrentInteractable == null) yield return null;
            Assert.That(investigator2.InvestigatorCard.CanBePlayed.IsTrue, Is.False);

            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();
            taskGameAction = _gameActionsProvider.Create<DefeatCardGameAction>().SetWith(creature, investigator.InvestigatorCard).Execute();
            yield return ClickedIn(investigator.InvestigatorCard);
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.Keys.Value, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator BlankAndUnblankCratureStarTokenBuff()
        {
            _ = MustBeRevealedThisToken(ChallengeTokenType.Star);
            Task<(int totalTokenAmount, int totalTokenValue)> totalTokensRevealed = CaptureTotalTokensRevelaed();

            yield return StartingScene();
            CardCreature creature = _cardsProvider.GetCard<Card01603>();
            Investigator investigator = _investigatorsProvider.Third;
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature, SceneCORE1.Study.OwnZone).Execute().AsCoroutine();
            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(SceneCORE1.Study);
            yield return ClickedMainButton();

            //int? challengeValue = null;
            //while (challengeValue == null)
            //{
            //    challengeValue ??= _gameActionsProvider.CurrentChallenge?.TokensRevealed.First().Value.Invoke(investigator);
            //    yield return null;
            //}

            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(totalTokensRevealed.Result.totalTokenValue, Is.EqualTo(0));

            _ = MustBeRevealedThisToken(ChallengeTokenType.Star);
            totalTokensRevealed = CaptureTotalTokensRevelaed();
            yield return _gameActionsProvider.Create<DefeatCardGameAction>().SetWith(creature, investigator.InvestigatorCard).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<ResetAllInvestigatorsTurnsGameAction>().Execute().AsCoroutine();
            taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(SceneCORE1.Study);
            yield return ClickedMainButton();

            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(totalTokensRevealed.Result.totalTokenValue, Is.EqualTo(2));
        }
    }
}
