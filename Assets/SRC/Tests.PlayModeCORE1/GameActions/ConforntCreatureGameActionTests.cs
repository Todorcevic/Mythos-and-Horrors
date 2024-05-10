using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class ConforntCreatureGameActionTests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator MoveConfrontCratureTest()
        {
            yield return _preparationSceneCORE1.StartingScene();

            CardCreature creature = _cardsProvider.GetCard<Card01116>();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, _preparationSceneCORE1.SceneCORE1.Study.OwnZone)).AsCoroutine();

            Assert.That(creature.CurrentZone, Is.EqualTo(_investigatorsProvider.First.DangerZone));
        }

        [UnityTest]
        public IEnumerator ExhaustConfrontCratureTest()
        {
            yield return _preparationSceneCORE1.StartingScene();
            CardCreature creature = _cardsProvider.GetCard<Card01116>();
            yield return _gameActionsProvider.Create(new UpdateStatesGameAction(creature.Exausted, true)).AsCoroutine();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, _preparationSceneCORE1.SceneCORE1.Study.OwnZone)).AsCoroutine();
            Assert.That(creature.CurrentZone, Is.EqualTo(_preparationSceneCORE1.SceneCORE1.Study.OwnZone));
            yield return _gameActionsProvider.Create(new ReadyAllCardsGameAction()).AsCoroutine();
            Assert.That(creature.CurrentZone, Is.EqualTo(_investigatorsProvider.First.DangerZone));
        }

        [UnityTest]
        public IEnumerator NoConfrontCratureTest()
        {
            yield return _preparationSceneCORE1.StartingScene();
            CardCreature creature = _cardsProvider.GetCard<Card01601>();
            yield return _gameActionsProvider.Create(new UpdateStatesGameAction(creature.Exausted, true)).AsCoroutine();
            yield return _gameActionsProvider.Create(new EliminateInvestigatorGameAction(_investigatorsProvider.Third)).AsCoroutine();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, _preparationSceneCORE1.SceneCORE1.Study.OwnZone)).AsCoroutine();
            Assert.That(creature.CurrentZone, Is.EqualTo(_preparationSceneCORE1.SceneCORE1.Study.OwnZone));
            yield return _gameActionsProvider.Create(new ReadyAllCardsGameAction()).AsCoroutine();
            Assert.That(creature.CurrentZone, Is.EqualTo(_preparationSceneCORE1.SceneCORE1.Study.OwnZone));
        }
    }
}
