using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardAdversity01596Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator ObligationDiscardAllButOne()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return BuildCard("01596", investigator);
            Card adversity = _cardsProvider.GetCard<Card01596>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card cardToMaintan = investigator.HandZone.Cards.First(card => card.CanBeDiscarted.IsTrue);
            Task taskGameAction = _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, adversity).Execute();
            yield return ClickedIn(cardToMaintan);
            yield return taskGameAction.AsCoroutine();

            Assert.That(_investigatorsProvider.First.HandZone.Cards.Count(), Is.EqualTo(1));
            Assert.That(_investigatorsProvider.First.HandZone.Cards.Unique(), Is.EqualTo(cardToMaintan));
        }
    }
}
