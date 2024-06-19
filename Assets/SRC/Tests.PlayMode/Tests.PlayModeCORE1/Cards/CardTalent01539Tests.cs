
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardTalent01539Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator TakeResources()
        {
            Assert.That(false, "Not Implemented");
            Investigator investigator = _investigatorsProvider.First;
            Card01539 cardTalent = _cardsProvider.GetCard<Card01539>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardTalent, investigator.HandZone)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedClone(cardTalent, 1);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.Resources.Value, Is.EqualTo(8));
        }
    }
}
