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
            Card adversity = BuilCard("01596");
            Investigator investigator = _investigatorsProvider.First;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new AddRequerimentCardGameAction(investigator, adversity)).AsCoroutine();
            Card cardToMaintan = investigator.HandZone.Cards.First(card => card.CanBeDiscarded);

            Task taskGameAction = _gameActionsProvider.Create(new DrawGameAction(investigator, adversity));
            yield return ClickedIn(cardToMaintan);
            yield return taskGameAction.AsCoroutine();

            Assert.That(_investigatorsProvider.First.HandZone.Cards.Count, Is.EqualTo(1));
            Assert.That(_investigatorsProvider.First.HandZone.Cards.Unique(), Is.EqualTo(cardToMaintan));
            investigator.RequerimentCard.Remove(adversity);
        }
    }
}
