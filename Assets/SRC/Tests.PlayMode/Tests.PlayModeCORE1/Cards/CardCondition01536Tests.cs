using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardCondition01536Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator ChangeChallenges()
        {
            Investigator investigator = _investigatorsProvider.Second;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value0);
            Task<ResultChallengeGameAction> challenge = CaptureResolvingChallenge();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01536 conditionCard = _cardsProvider.GetCard<Card01536>();

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(conditionCard, investigator.HandZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.GhoulSecuaz, investigator.DangerZone).Start().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Start();
            yield return ClickedIn(conditionCard);
            yield return ClickedClone(SceneCORE1.GhoulSecuaz, 0);
            yield return ClickedIn(conditionCard);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(challenge.Result.ChallengePhaseGameAction.Stat, Is.EqualTo(investigator.Intelligence));
        }
    }
}
