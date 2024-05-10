using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class MoveCreatureGameActionTests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator MoveCratureTest()
        {
            yield return _preparationSceneCORE1.StartingScene();
            CardPlace place2 = _cardsProvider.GetCard<Card01112>();
            CardPlace place3 = _cardsProvider.GetCard<Card01113>();
            CardPlace place4 = _cardsProvider.GetCard<Card01114>();
            CardCreature creature = _cardsProvider.GetCard<Card01116>();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, place2.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.First.AvatarCard, place3.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.Second.AvatarCard, place4.OwnZone)).AsCoroutine();

            yield return _gameActionsProvider.Create(new MoveCreatureGameAction((IStalker)creature)).AsCoroutine();

            if (DEBUG_MODE) yield return PressAnyKey();
            Assert.That(creature.CurrentPlace, Is.EqualTo(place3));
        }

        [UnityTest]
        public IEnumerator CantMoveTargetCratureTest()
        {
            yield return _preparationSceneCORE1.PlaceAllScene();
            yield return _preparationSceneCORE1.PlayThisInvestigator(_investigatorsProvider.First);
            yield return _preparationSceneCORE1.PlayThisInvestigator(_investigatorsProvider.Second);
            CardCreature creature = _cardsProvider.GetCard<Card01601>();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, _preparationSceneCORE1.SceneCORE1.Hallway.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.First.AvatarCard, _preparationSceneCORE1.SceneCORE1.Attic.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.Second.AvatarCard, _preparationSceneCORE1.SceneCORE1.Cellar.OwnZone)).AsCoroutine();

            yield return _gameActionsProvider.Create(new MoveCreatureGameAction((IStalker)creature)).AsCoroutine();

            if (DEBUG_MODE) yield return PressAnyKey();
            Assert.That(creature.CurrentPlace, Is.EqualTo(_preparationSceneCORE1.SceneCORE1.Hallway));
        }

        [UnityTest]
        public IEnumerator MoveTargetCratureTest()
        {
            yield return _preparationSceneCORE1.StartingScene();
            CardPlace place2 = _cardsProvider.GetCard<Card01112>();
            CardPlace place3 = _cardsProvider.GetCard<Card01113>();
            CardPlace place4 = _cardsProvider.GetCard<Card01114>();
            CardPlace place5 = _cardsProvider.GetCard<Card01115>();
            CardCreature creature = _cardsProvider.GetCard<Card01601>();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, place2.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.First.AvatarCard, place3.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.Second.AvatarCard, place4.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.Third.AvatarCard, place5.OwnZone)).AsCoroutine();

            yield return _gameActionsProvider.Create(new MoveCreatureGameAction((IStalker)creature)).AsCoroutine();

            if (DEBUG_MODE) yield return PressAnyKey();
            Assert.That(creature.CurrentPlace, Is.EqualTo(place5));
        }
    }
}
