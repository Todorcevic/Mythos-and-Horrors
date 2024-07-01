using MythosAndHorrors.GameRules;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using MythosAndHorrors.PlayMode.Tests;
using System.Threading.Tasks;
using System.Linq;

namespace MythosAndHorrors.PlayModeCORE3.Tests
{

    public class CardCreature01177Tests : TestCORE3Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator DiscarWithAttack()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01177 creature = _cardsProvider.GetCard<Card01177>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature, investigator.DangerZone).Start().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new CreatureAttackGameAction(creature, investigator));
            yield return ClickedIn(investigator.DiscardableCardsInHand.First());
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.DamageRecived.Value, Is.EqualTo(1));
            Assert.That(investigator.FearRecived.Value, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator CantDiscardWithAttack()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01177 creature = _cardsProvider.GetCard<Card01177>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature, investigator.DangerZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator.HandZone.Cards, investigator.DiscardZone).Start().AsCoroutine();

            yield return _gameActionsProvider.Create(new CreatureAttackGameAction(creature, investigator)).AsCoroutine();

            Assert.That(investigator.DamageRecived.Value, Is.EqualTo(2));
            Assert.That(investigator.FearRecived.Value, Is.EqualTo(2));
            Assert.That(creature.Damage.Value, Is.EqualTo(1));
            Assert.That(creature.Fear.Value, Is.EqualTo(1));
        }
    }
}
