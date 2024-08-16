
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01527Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator ExtraTurns()
        {
            Investigator investigator = _investigatorsProvider.First;
            Investigator investigator2 = _investigatorsProvider.Second;
            yield return BuildCard("01527", investigator);
            Card01527 cardSupply = _cardsProvider.GetCard<Card01527>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return PlayThisInvestigator(investigator2);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardSupply, investigator.AidZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator2).Execute();
            yield return ClickedIn(cardSupply);
            yield return ClickedTokenButton();
            yield return ClickedTokenButton();
            yield return ClickedTokenButton();
            yield return ClickedTokenButton();
            yield return ClickedTokenButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator2.Resources.Value, Is.EqualTo(10));
        }
    }
}
