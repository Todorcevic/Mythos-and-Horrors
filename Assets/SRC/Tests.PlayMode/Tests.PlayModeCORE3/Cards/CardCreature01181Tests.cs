
using MythosAndHorrors.GameRules;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE3.Tests
{
    public class CardCreature01181Tests : TestCORE3Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator DoFearWhenConfront()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01181 creature = _cardsProvider.GetCard<Card01181>();
            yield return _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(creature, investigator).Start().AsCoroutine();

            Assert.That(investigator.FearRecived.Value, Is.EqualTo(1));
        }
    }
}
