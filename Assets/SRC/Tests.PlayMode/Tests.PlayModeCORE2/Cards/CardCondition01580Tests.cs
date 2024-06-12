using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using System.Threading.Tasks;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardCondition01580Tests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator UpdateStatModifierChallenge()
        {
            Investigator investigator = _investigatorsProvider.Fourth;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_4);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01580 conditionCard = _cardsProvider.GetCard<Card01580>();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(conditionCard, investigator.HandZone)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return ClickedIn(conditionCard);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.Hints.Value, Is.EqualTo(1));
        }
    }
}
