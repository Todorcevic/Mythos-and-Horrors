
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardTalent01553Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator ReturnToHand()
        {
            Investigator investigator = _investigatorsProvider.Second;
            Card01553 cardTalent = _cardsProvider.GetCard<Card01553>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardTalent, investigator.HandZone)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedIn(cardTalent);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(cardTalent.CurrentZone, Is.EqualTo(investigator.HandZone));
        }

        [UnityTest]
        public IEnumerator CantCommit()
        {
            Investigator investigator = _investigatorsProvider.First;
            Investigator investigator2 = _investigatorsProvider.Second;
            Card01553 cardTalent = _cardsProvider.GetCard<Card01553>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return PlayThisInvestigator(investigator2);

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardTalent, investigator2.HandZone)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(investigator.CurrentPlace);
            yield return AssertThatIsNotClickable(cardTalent);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(cardTalent.CurrentZone, Is.EqualTo(investigator2.HandZone));
        }
    }
}
