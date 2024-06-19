
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
        public IEnumerator TakeResources()
        {
            Assert.That(false, "Not Implemented");
            Investigator investigator = _investigatorsProvider.First;
            Card01689 cardSupply = _cardsProvider.GetCard<Card01689>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardSupply, investigator.HandZone)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(cardSupply);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.Resources.Value, Is.EqualTo(8));
        }
    }
}
