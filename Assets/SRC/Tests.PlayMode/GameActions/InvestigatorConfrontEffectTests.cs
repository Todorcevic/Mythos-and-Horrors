using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class InvestigatorConfrontEffectTests : TestBase
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator InvestigatorConfrontTest()
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
            if (!DEBUG_MODE) yield return WaitToCloneClick(oiGA.InvestigatorConfrontEffects.Find(effect => effect.CardAffected == creature));

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(creature.CurrentZone, Is.EqualTo(_investigatorsProvider.First.DangerZone));
        }
    }
}
