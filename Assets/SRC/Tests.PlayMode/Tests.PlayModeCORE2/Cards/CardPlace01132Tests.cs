
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardPlace01132Tests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator DecreaseAllyCosts()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01521 support = _cardsProvider.GetCard<Card01521>();
            Card01132 cardPlace = _cardsProvider.GetCard<Card01132>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, cardPlace)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(support, investigator.HandZone)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(support);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.Resources.Value, Is.EqualTo(4));
            Assert.That(support.CurrentZone, Is.EqualTo(investigator.AidZone));
        }
    }
}
