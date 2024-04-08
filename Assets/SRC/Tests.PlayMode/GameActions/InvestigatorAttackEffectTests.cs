using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class InvestigatorAttackEffectTests : TestBase
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator InvestigatorAttackInDangerZoneTest()
        {
            CardCreature creature = _cardsProvider.AllCards.OfType<CardCreature>().First();
            CardPlace place = _cardsProvider.AllCards.OfType<CardPlace>().First();
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(_investigatorsProvider.First.CurrentTurns, GameValues.DEFAULT_TURNS_AMOUNT)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place, _chaptersProvider.CurrentScene.PlaceZone[2, 2])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, _investigatorsProvider.First.DangerZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.First.AvatarCard, place.OwnZone)).AsCoroutine();

            OneInvestigatorTurnGameAction oneInvestigatorTurnGameAction = new(_investigatorsProvider.First);
            _ = _gameActionsProvider.Create(oneInvestigatorTurnGameAction);
            if (!DEBUG_MODE) yield return WaitToClick(creature);
            if (!DEBUG_MODE) yield return WaitToCloneClick(oneInvestigatorTurnGameAction.InvestigatorAttackEffects.Find(effect => effect.CardAffected == creature));

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(creature.Health.Value, Is.EqualTo(creature.Info.Health - 1));
        }

        [UnityTest]
        public IEnumerator InvestigatorAttackInPlaceZoneTest()
        {
            CardCreature creature = _cardsProvider.AllCards.OfType<CardCreature>().First();
            CardPlace place = _cardsProvider.AllCards.OfType<CardPlace>().First();
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(_investigatorsProvider.First.CurrentTurns, GameValues.DEFAULT_TURNS_AMOUNT)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place, _chaptersProvider.CurrentScene.PlaceZone[2, 2])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, place.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.First.AvatarCard, place.OwnZone)).AsCoroutine();

            OneInvestigatorTurnGameAction oiGA = new(_investigatorsProvider.First);
            _ = _gameActionsProvider.Create(oiGA);

            if (!DEBUG_MODE) yield return WaitToClick(creature);
            if (!DEBUG_MODE) yield return WaitToCloneClick(oiGA.InvestigatorAttackEffects.Find(effect => effect.CardAffected == creature));

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(creature.Health.Value, Is.EqualTo(creature.Info.Health - 1));
        }

        [UnityTest]
        public IEnumerator CantInvestigatorAttackTest()
        {
            CardCreature creature = _cardsProvider.AllCards.OfType<CardCreature>().First();
            CardPlace place = _cardsProvider.AllCards.OfType<CardPlace>().First();
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(_investigatorsProvider.First.CurrentTurns, GameValues.DEFAULT_TURNS_AMOUNT)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place, _chaptersProvider.CurrentScene.PlaceZone[2, 2])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, place.OwnZone)).AsCoroutine();

            OneInvestigatorTurnGameAction oiGA = new(_investigatorsProvider.First);
            _ = _gameActionsProvider.Create(oiGA);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(!oiGA.InvestigatorAttackEffects.Exists(effect => effect.CardAffected == creature));
        }
    }
}
