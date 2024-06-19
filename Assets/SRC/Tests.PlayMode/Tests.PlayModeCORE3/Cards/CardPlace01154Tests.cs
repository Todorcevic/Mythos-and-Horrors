
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE3.Tests
{
    public class CardPlace01154Tests : TestCORE3Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator TakeResources()
        {
            Assert.That(false, "Not Implemented");
            Investigator investigator = _investigatorsProvider.First;
            Card01154 cardPlace = _cardsProvider.GetCard<Card01154>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, cardPlace)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedClone(cardPlace, 1);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.Resources.Value, Is.EqualTo(8));
        }
    }
}
