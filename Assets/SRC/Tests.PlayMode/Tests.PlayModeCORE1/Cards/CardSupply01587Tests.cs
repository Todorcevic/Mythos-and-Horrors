﻿
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01587Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator DecrementEnigma()
        {
            Investigator investigator = _investigatorsProvider.First;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_2);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01587 supplyCard = _cardsProvider.GetCard<Card01587>();

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supplyCard, investigator.AidZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(supplyCard);
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();
            yield return ClickedResourceButton();
            yield return ClickedResourceButton();
            yield return AssertThatIsNotClickable(supplyCard);
            yield return ClickedMainButton();

            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.Keys.Value, Is.EqualTo(1));
            Assert.That(supplyCard.Charge.Amount.Value, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator DecrementEnigmaWhenEnigmaIsLow()
        {
            Investigator investigator = _investigatorsProvider.First;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_4);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01587 supplyCard = _cardsProvider.GetCard<Card01587>();

            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, SceneCORE1.Hallway).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supplyCard, investigator.AidZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(supplyCard);
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.Keys.Value, Is.EqualTo(0));
            Assert.That(supplyCard.Charge.Amount.Value, Is.EqualTo(2));
            Assert.That(investigator.CurrentPlace.Enigma.Value, Is.EqualTo(1));
        }
    }
}
