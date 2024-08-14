using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{

    public class CardAdversity01166Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator DecrementStat()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01166 cardAdversity = _cardsProvider.GetCard<Card01166>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, cardAdversity).Execute().AsCoroutine();


            Assert.That(SceneCORE1.CurrentPlot.AmountDecrementedEldritch, Is.EqualTo(1));
        }
    }
}