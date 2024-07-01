using MythosAndHorrors.GameRules;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE3.Tests
{

    public class CardCreature01180Tests : TestCORE3Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator DoFearWhenDefeat()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return PlayThisInvestigator(_investigatorsProvider.Second);
            Card01180 creature = _cardsProvider.GetCard<Card01180>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature, investigator.DangerZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create(new DefeatCardGameAction(creature, investigator.InvestigatorCard)).AsCoroutine();

            Assert.That(investigator.FearRecived.Value, Is.EqualTo(1));
            Assert.That(_investigatorsProvider.Second.FearRecived.Value, Is.EqualTo(1));
        }
    }
}
