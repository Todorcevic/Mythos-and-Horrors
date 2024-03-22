using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class ChooseInvestigatorGameActionTest : TestBase
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly ChaptersProvider _chapterProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator ChooseInvestigatorTest()
        {
            yield return PlayAllInvestigators();
            CardPlace place = _cardsProvider.GetCard<CardPlace>("01111");
            _investigatorsProvider.AllInvestigatorsInPlay.ForEach(investigator =>
            _gameActionsProvider.Create(new UpdateStatGameAction(investigator.CurrentTurns, GameValues.DEFAULT_TURNS_AMOUNT)).AsCoroutine());

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place, _chapterProvider.CurrentScene.PlaceZone[0, 4])).AsCoroutine();
            if (!DEBUG_MODE) WaitToHistoryPanelClick().AsTask();

            IEnumerable<Card> allAvatars = _investigatorsProvider.AllInvestigatorsInPlay.Select(investigator => investigator.AvatarCard).Cast<Card>();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(allAvatars, place.OwnZone)).AsCoroutine();

            if (!DEBUG_MODE) WaitToClick(_investigatorsProvider.First.AvatarCard).AsTask();
            ChooseInvestigatorGameAction chooseInvestigatoGA = new(_investigatorsProvider.GetInvestigatorsCanStartTurn);
            yield return _gameActionsProvider.Create(chooseInvestigatoGA).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(chooseInvestigatoGA.InvestigatorSelected, Is.EqualTo(_investigatorsProvider.First));
        }
    }
}
