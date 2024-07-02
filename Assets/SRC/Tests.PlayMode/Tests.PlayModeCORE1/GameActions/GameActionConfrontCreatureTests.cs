using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class GameActionConfrontCreatureTests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator MoveConfrontCratureTest()
        {
            yield return StartingScene();

            CardCreature creature = _cardsProvider.GetCard<Card01116>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature, SceneCORE1.Study.OwnZone).Start().AsCoroutine();

            Assert.That(creature.CurrentZone, Is.EqualTo(_investigatorsProvider.First.DangerZone));
        }

        [UnityTest]
        public IEnumerator ExhaustConfrontCratureTest()
        {
            yield return StartingScene();
            CardCreature creature = _cardsProvider.GetCard<Card01116>();
            yield return _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(creature.Exausted, true).Start().AsCoroutine();

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature, SceneCORE1.Study.OwnZone).Start().AsCoroutine();
            Assert.That(creature.CurrentZone, Is.EqualTo(SceneCORE1.Study.OwnZone));
            yield return _gameActionsProvider.Create<ReadyAllCardsGameAction>().Start().AsCoroutine();
            Assert.That(creature.CurrentZone, Is.EqualTo(_investigatorsProvider.First.DangerZone));
        }

        [UnityTest]
        public IEnumerator NoConfrontCratureTest()
        {
            yield return StartingScene();
            CardCreature creature = _cardsProvider.GetCard<Card01601>();
            yield return _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(creature.Exausted, true).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<EliminateInvestigatorGameAction>().SetWith(_investigatorsProvider.Third).Start().AsCoroutine();

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature, SceneCORE1.Study.OwnZone).Start().AsCoroutine();
            Assert.That(creature.CurrentZone, Is.EqualTo(SceneCORE1.Study.OwnZone));
            yield return _gameActionsProvider.Create<ReadyAllCardsGameAction>().Start().AsCoroutine();
            Assert.That(creature.CurrentZone, Is.EqualTo(SceneCORE1.Study.OwnZone));
        }
    }
}
