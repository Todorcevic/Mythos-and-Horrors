using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01533Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator GainResourceWhenInvestigate()
        {
            Investigator investigator = _investigatorsProvider.Second;
            Card01533 cardSupply = _cardsProvider.GetCard<Card01533>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value0);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardSupply, investigator.AidZone).Execute().AsCoroutine();
            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();
            yield return ClickedIn(cardSupply);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.Hints.Value, Is.EqualTo(1));
            Assert.That(investigator.Resources.Value, Is.EqualTo(6));
            Assert.That(investigator.Intelligence.Value, Is.EqualTo(6));
        }
    }
}
