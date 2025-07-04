﻿using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardCondition01556Tests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator SwapTokenValue()
        {
            Investigator investigator = _investigatorsProvider.Third;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_4).ContinueWith((_) => MustBeRevealedThisToken(ChallengeTokenType.Value_4));
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return BuildCard("01556", investigator);
            Card01556 conditionCard = _cardsProvider.GetCard<Card01556>();

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(conditionCard, investigator.HandZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();
            yield return ClickedIn(conditionCard);
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.Keys.Value, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator DualCardsInHandSwapTokenValue()
        {
            Investigator investigator = _investigatorsProvider.Third;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_4).ContinueWith((_) => MustBeRevealedThisToken(ChallengeTokenType.Value_4));
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return BuildCard("01556", investigator);
            yield return BuildCard("01556", investigator);
            IEnumerable<Card01556> conditionCards = _cardsProvider.GetCards<Card01556>();

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(conditionCards, investigator.HandZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();
            yield return ClickedIn(conditionCards.First());
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.Keys.Value, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator SwapSceneTokenValue()
        {
            Investigator investigator = _investigatorsProvider.Third;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Danger).ContinueWith((_) => MustBeRevealedThisToken(ChallengeTokenType.Danger));
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return BuildCard("01556", investigator);
            Card01556 conditionCard = _cardsProvider.GetCard<Card01556>();

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(conditionCard, investigator.HandZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();
            yield return ClickedIn(conditionCard);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.Keys.Value, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator SwapTokenValueWithRetry()
        {
            Investigator investigator = _investigatorsProvider.Fourth;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_4)
                .ContinueWith((_) => MustBeRevealedThisToken(ChallengeTokenType.Value_4)
                .ContinueWith((_) => MustBeRevealedThisToken(ChallengeTokenType.Value_4)));
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return BuildCard("01556", investigator);
            Card01556 conditionCard = _cardsProvider.GetCard<Card01556>();

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(conditionCard, investigator.HandZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();
            yield return ClickedIn(investigator.InvestigatorCard);
            yield return ClickedIn(investigator.HandZone.Cards.First(card => card.CanBeDiscarted.IsTrue && card != conditionCard));
            yield return ClickedIn(conditionCard);
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.Keys.Value, Is.EqualTo(1));
        }
    }
}
