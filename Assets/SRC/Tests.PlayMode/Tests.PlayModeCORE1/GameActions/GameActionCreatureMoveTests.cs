﻿using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class GameActionCreatureMoveTests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator MoveCratureTest()
        {
            yield return StartingScene();
            CardPlace place2 = _cardsProvider.GetCard<Card01112>();
            CardPlace place3 = _cardsProvider.GetCard<Card01113>();
            CardPlace place4 = _cardsProvider.GetCard<Card01114>();
            CardCreature creature = _cardsProvider.GetCard<Card01116>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature, place2.OwnZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(_investigatorsProvider.First.AvatarCard, place3.OwnZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(_investigatorsProvider.Second.AvatarCard, place4.OwnZone).Execute().AsCoroutine();

            yield return _gameActionsProvider.Create<MoveCreatureGameAction>().SetWith((IStalker)creature, _investigatorsProvider.AllInvestigatorsInPlay).Execute().AsCoroutine();

            Assert.That(creature.CurrentPlace, Is.EqualTo(place3));
        }

        [UnityTest]
        public IEnumerator CantMoveTargetCratureTest()
        {
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(_investigatorsProvider.First);
            yield return PlayThisInvestigator(_investigatorsProvider.Second);
            CardCreature creature = _cardsProvider.GetCard<Card01601>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature, SceneCORE1.Hallway.OwnZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(_investigatorsProvider.First.AvatarCard, SceneCORE1.Attic.OwnZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(_investigatorsProvider.Second.AvatarCard, SceneCORE1.Cellar.OwnZone).Execute().AsCoroutine();

            yield return _gameActionsProvider.Create<MoveCreatureGameAction>().SetWith((IStalker)creature, _investigatorsProvider.AllInvestigatorsInPlay).Execute().AsCoroutine();

            Assert.That(creature.CurrentPlace, Is.EqualTo(SceneCORE1.Hallway));
        }

        [UnityTest]
        public IEnumerator MoveTargetCratureTest()
        {
            yield return StartingScene();
            CardPlace place2 = _cardsProvider.GetCard<Card01112>();
            CardPlace place3 = _cardsProvider.GetCard<Card01113>();
            CardPlace place4 = _cardsProvider.GetCard<Card01114>();
            CardPlace place5 = _cardsProvider.GetCard<Card01115>();
            CardCreature creature = _cardsProvider.GetCard<Card01601>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature, place2.OwnZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(_investigatorsProvider.First.AvatarCard, place3.OwnZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(_investigatorsProvider.Second.AvatarCard, place4.OwnZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(_investigatorsProvider.Third.AvatarCard, place5.OwnZone).Execute().AsCoroutine();

            yield return _gameActionsProvider.Create<MoveCreatureGameAction>().SetWith((IStalker)creature, _investigatorsProvider.AllInvestigatorsInPlay).Execute().AsCoroutine();

            Assert.That(creature.CurrentPlace, Is.EqualTo(place5));
        }
    }
}
