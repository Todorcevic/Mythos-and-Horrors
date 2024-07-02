using MythosAndHorrors.GameRules;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using MythosAndHorrors.PlayMode.Tests;
using System.Threading.Tasks;

namespace MythosAndHorrors.PlayModeCORE3.Tests
{

    public class CardCreature01179Tests : TestCORE3Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator Health()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01179 creature = _cardsProvider.GetCard<Card01179>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature, investigator.DangerZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<HarmToCardGameAction>().SetWith(creature, investigator.InvestigatorCard, amountDamage: 3).Start().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new RoundGameAction());
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(creature.DamageRecived.Value, Is.EqualTo(1));
        }
    }
}
