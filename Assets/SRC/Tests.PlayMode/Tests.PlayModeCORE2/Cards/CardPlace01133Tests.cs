
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardPlace01133Tests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator EnterWin()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01133 cardPlace = _cardsProvider.GetCard<Card01133>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(cardPlace);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.CurrentPlace, Is.EqualTo(cardPlace));
        }

        [UnityTest]
        public IEnumerator EnterFail()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01133 cardPlace = _cardsProvider.GetCard<Card01133>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_1);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(cardPlace);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.CurrentPlace, Is.EqualTo(SceneCORE2.Fluvial));
            Assert.That(investigator.FearRecived.Value, Is.EqualTo(2));
        }
    }
}
