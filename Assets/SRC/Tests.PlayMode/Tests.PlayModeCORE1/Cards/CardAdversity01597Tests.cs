using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardAdversity01597Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator ObligationPayAllResources()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return BuildCard("01597", investigator);
            Card adversity = _cardsProvider.GetCard<Card01597>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator, withResources: true);

            Assert.That(_investigatorsProvider.First.Resources.Value, Is.EqualTo(5));
            yield return _gameActionsProvider.Create(new DrawGameAction(investigator, adversity)).AsCoroutine();

            Assert.That(_investigatorsProvider.First.Resources.Value, Is.EqualTo(0));
        }
    }
}
