using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class MoveCreatureGameActionTests : TestBase
    {

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator MoveCratureTest()
        {
            yield return _preparationScene.PlayAllInvestigators();
            yield return _preparationScene.PlaceAllPlaceCards();
            CardPlace place2 = _cardsProvider.GetCard<Card01112>();
            CardPlace place3 = _cardsProvider.GetCard<Card01113>();
            CardPlace place4 = _cardsProvider.GetCard<Card01114>();
            CardCreature creature = _cardsProvider.GetCard<Card01116>();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, place2.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.First.AvatarCard, place3.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.Second.AvatarCard, place4.OwnZone)).AsCoroutine();

            yield return _gameActionsProvider.Create(new MoveCreatureGameAction((IStalker)creature)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(creature.CurrentPlace, Is.EqualTo(place3));
        }
    }
}
