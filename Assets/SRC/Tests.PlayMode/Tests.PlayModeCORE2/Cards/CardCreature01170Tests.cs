using MythosAndHorrors.GameRules;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardCreature01170Tests : TestCORE2Preparation
    {
        [UnityTest]
        public IEnumerator GainEldritch()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01170 acolit = _cardsProvider.GetCard<Card01170>();

            yield return _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(acolit).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<ScenePhaseGameAction>().Execute().AsCoroutine();

            Assert.That(acolit.Eldritch.Value, Is.EqualTo(1));
            Assert.That(SceneCORE2.CurrentPlot.Eldritch.Value, Is.EqualTo(5)); //-1 for ScenePhase ,-1 for Acolit 
            Assert.That(acolit.CurrentPlace, Is.Not.EqualTo(investigator.CurrentPlace));
        }
    }
}
