using MythosAndHorrors.GameRules;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE3.Tests
{

    public class CardCreature01175Tests : TestCORE3Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator ActiveBuff()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01175 creature = _cardsProvider.GetCard<Card01175>();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, investigator.DangerZone)).AsCoroutine();
            Assert.That(creature.Strength.Value, Is.EqualTo(3));
            Assert.That(creature.Agility.Value, Is.EqualTo(3));

            yield return _gameActionsProvider.Create(new HarmToInvestigatorGameAction(investigator, creature, amountFear: 3)).AsCoroutine();
            Assert.That(creature.Strength.Value, Is.EqualTo(4));
            Assert.That(creature.Agility.Value, Is.EqualTo(4));

            yield return _gameActionsProvider.Create(new EludeGameAction(creature)).AsCoroutine();
            Assert.That(creature.Strength.Value, Is.EqualTo(3));
            Assert.That(creature.Agility.Value, Is.EqualTo(3));
        }
    }
}
