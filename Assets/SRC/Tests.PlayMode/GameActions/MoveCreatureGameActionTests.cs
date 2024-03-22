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
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator MoveCratureTest()
        {
            yield return PlayAllInvestigators();
            CardPlace place1 = _cardsProvider.GetCard<CardPlace>("01111");
            CardPlace place2 = _cardsProvider.GetCard<CardPlace>("01112");
            CardPlace place3 = _cardsProvider.GetCard<CardPlace>("01113");
            CardPlace place4 = _cardsProvider.GetCard<CardPlace>("01114");
            CardPlace place5 = _cardsProvider.GetCard<CardPlace>("01115");
            CardCreature creature = _cardsProvider.GetCard<CardCreature>("01116");
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place1, _chaptersProvider.CurrentScene.PlaceZone[0, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place2, _chaptersProvider.CurrentScene.PlaceZone[1, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place3, _chaptersProvider.CurrentScene.PlaceZone[0, 4])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place4, _chaptersProvider.CurrentScene.PlaceZone[1, 4])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place5, _chaptersProvider.CurrentScene.PlaceZone[2, 4])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, place2.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.First.AvatarCard, place3.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.Second.AvatarCard, place4.OwnZone)).AsCoroutine();

            yield return _gameActionsProvider.Create(new MoveCreatureGameAction((IStalker)creature)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(creature.CurrentPlace, Is.EqualTo(place3));
        }
    }
}
