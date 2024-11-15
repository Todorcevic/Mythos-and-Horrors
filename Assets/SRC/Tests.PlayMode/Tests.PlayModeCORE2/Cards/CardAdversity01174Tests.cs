﻿using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardAdversity01174Tests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator SolvingChallenge()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01174 cardAdversity = _cardsProvider.GetCard<Card01174>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1).ContinueWith((_) => MustBeRevealedThisToken(ChallengeTokenType.Value1));
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, cardAdversity).Execute().AsCoroutine();
            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(cardAdversity);
            yield return ClickedClone(cardAdversity, 0, true);
            yield return ClickedMainButton();
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(cardAdversity.CurrentZone, Is.EqualTo(SceneCORE2.DangerDiscardZone));
            Assert.That(investigator.Keys.Value, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator TrySpecialInvestigate()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01174 cardAdversity = _cardsProvider.GetCard<Card01174>();
            Card01587 lantern = _cardsProvider.GetCard<Card01587>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1).ContinueWith((_) => MustBeRevealedThisToken(ChallengeTokenType.Value1));
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(lantern, investigator.AidZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, cardAdversity).Execute().AsCoroutine();
            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return AssertThatIsNotClickable(investigator.CurrentPlace);
            yield return AssertThatIsNotClickable(lantern);
            yield return ClickedIn(cardAdversity);
            yield return ClickedClone(cardAdversity, 0, true);
            yield return ClickedMainButton();
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(cardAdversity.CurrentZone, Is.EqualTo(SceneCORE2.DangerDiscardZone));
            Assert.That(investigator.Keys.Value, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator TrySpecialInvestigate2()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return BuildCard("01685", investigator);
            Card01174 cardAdversity = _cardsProvider.GetCard<Card01174>();
            Card01685 cardCondition = _cardsProvider.GetCard<Card01685>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1).ContinueWith((_) => MustBeRevealedThisToken(ChallengeTokenType.Value1));
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardCondition, investigator.HandZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, cardAdversity).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return AssertThatIsNotClickable(investigator.CurrentPlace);
            yield return AssertThatIsNotClickable(cardCondition);
            yield return ClickedIn(cardAdversity);
            yield return ClickedClone(cardAdversity, 0, true);
            yield return ClickedMainButton();
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(cardAdversity.CurrentZone, Is.EqualTo(SceneCORE2.DangerDiscardZone));
            Assert.That(investigator.Keys.Value, Is.EqualTo(1));
        }
    }
}