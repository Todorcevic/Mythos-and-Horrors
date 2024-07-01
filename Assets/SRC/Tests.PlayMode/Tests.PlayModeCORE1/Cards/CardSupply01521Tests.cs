
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01521Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator BackDamage()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01521 cardSupply = _cardsProvider.GetCard<Card01521>();
            CardCreature creature = SceneCORE1.GhoulSecuaz;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardSupply, investigator.AidZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature, investigator.DangerZone).Start().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new CreatureAttackGameAction(creature, investigator));
            yield return ClickedIn(cardSupply);
            yield return ClickedIn(cardSupply);
            yield return gameActionTask.AsCoroutine();

            Assert.That(creature.DamageRecived.Value, Is.EqualTo(1));
            Assert.That(cardSupply.CurrentZone, Is.EqualTo(investigator.DiscardZone));
        }
    }
}
