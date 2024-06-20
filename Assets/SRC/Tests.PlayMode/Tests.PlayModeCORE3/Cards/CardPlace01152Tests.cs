
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE3.Tests
{
    public class CardPlace01152Tests : TestCORE3Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator ChallengeWithPower()
        {
            Investigator investigator = _investigatorsProvider.Third;
            Card01152 cardPlace = _cardsProvider.GetCard<Card01152>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_1);

            //Task<ResultChallengeGameAction> challengePhaseGameAction = CaptureResolvingChallenge();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, cardPlace)).AsCoroutine();
            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(cardPlace);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.Hints.Value, Is.EqualTo(0));
        }
    }
}
