using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardAdversity01163Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator FailChallenge()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01163 cardAdversity = _cardsProvider.GetCard<Card01163>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_2);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            Task drawTask = _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, cardAdversity).Start();
            yield return ClickedMainButton();
            yield return drawTask.AsCoroutine();

            Assert.That(investigator.FearRecived.Value, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator WithAutoFail()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01163 cardAdversity = _cardsProvider.GetCard<Card01163>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Fail);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            Task drawTask = _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, cardAdversity).Start();
            yield return ClickedMainButton();
            yield return drawTask.AsCoroutine();

            Assert.That(investigator.FearRecived.Value, Is.EqualTo(3));
        }
    }
}


