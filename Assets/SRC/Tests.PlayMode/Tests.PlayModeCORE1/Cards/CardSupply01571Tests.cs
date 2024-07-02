
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01571Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator ChooseToken()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return BuildCard("01571", investigator);
            Card01571 supply = _cardsProvider.GetCard<Card01571>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_1).ContinueWith((_) => MustBeRevealedThisToken(ChallengeTokenType.Value_2));

            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supply, investigator.AidZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();
            yield return ClickedIn(supply);
            yield return ClickedClone(supply, 0, true);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(supply.Charge.Amount.Value, Is.EqualTo(3));
            Assert.That(investigator.Hints.Value, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator ChooseFailToken()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return BuildCard("01571", investigator);
            Card01571 supply = _cardsProvider.GetCard<Card01571>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_1).ContinueWith((_) => MustBeRevealedThisToken(ChallengeTokenType.Value_2));

            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supply, investigator.AidZone).Execute().AsCoroutine();
            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();
            yield return ClickedIn(supply);
            yield return ClickedClone(supply, 1, true);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.Hints.Value, Is.EqualTo(0));
        }
    }
}
