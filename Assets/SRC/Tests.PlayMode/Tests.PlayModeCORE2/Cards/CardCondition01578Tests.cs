using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using System.Threading.Tasks;
using System.Linq;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardCondition01578Tests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator Evade()
        {
            Investigator investigator = _investigatorsProvider.Fourth;
            Investigator investigator2 = _investigatorsProvider.Second;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return PlayThisInvestigator(investigator2);
            Card01578 conditionCard = _cardsProvider.GetCard<Card01578>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(conditionCard, investigator.HandZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE2.Drew, investigator.DangerZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE2.Herman, investigator2.DangerZone).Start().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(conditionCard);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(SceneCORE2.Drew.Exausted.IsActive, Is.True);
            Assert.That(SceneCORE2.Herman.Exausted.IsActive, Is.True);
            Assert.That(investigator.DamageRecived.Value, Is.EqualTo(0));
            Assert.That(investigator.AllTypeCreaturesConfronted.Any(), Is.False);
            Assert.That(investigator2.AllTypeCreaturesConfronted.Any(), Is.False);
        }
    }
}
