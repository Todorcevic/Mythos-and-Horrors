using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01689Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator SpecialInvestigation()
        {
            Investigator investigator = _investigatorsProvider.Fourth;
            yield return BuildCard("01689", investigator);
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_4);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01689 assetCard = _cardsProvider.GetCard<Card01689>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(assetCard, investigator.AidZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(assetCard);
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.Keys.Value, Is.EqualTo(2));
            Assert.That(assetCard.Charge.Amount.Value, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator WasteTurns()
        {
            Investigator investigator = _investigatorsProvider.Fourth;
            yield return BuildCard("01689", investigator);
            _ = MustBeRevealedThisToken(ChallengeTokenType.Fail);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return PlayThisInvestigator(_investigatorsProvider.Second);
            Card01689 assetCard = _cardsProvider.GetCard<Card01689>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(assetCard, investigator.AidZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<RoundGameAction>().Execute();
            yield return ClickedIn(investigator.AvatarCard);
            yield return ClickedIn(assetCard);
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();
            yield return ClickedIn(_investigatorsProvider.Second.AvatarCard);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.CurrentActions.ValueBeforeUpdate, Is.EqualTo(0));
            Assert.That(investigator.Keys.Value, Is.EqualTo(0));
            Assert.That(assetCard.Charge.Amount.Value, Is.EqualTo(2));
        }
    }
}
