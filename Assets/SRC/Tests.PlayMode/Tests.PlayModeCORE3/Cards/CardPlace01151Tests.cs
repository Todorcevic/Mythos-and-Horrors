
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE3.Tests
{
    public class CardPlace01151Tests : TestCORE3Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator CancelMove()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01151 cardPlace = _cardsProvider.GetCard<Card01151>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_1);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(cardPlace);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.CurrentPlace, Is.EqualTo(SceneCORE3.MainPath));
        }
    }
}
