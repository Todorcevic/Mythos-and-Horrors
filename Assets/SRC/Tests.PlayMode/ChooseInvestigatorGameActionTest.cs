using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
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
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly ChaptersProvider _chapterProvider;
        [Inject] private readonly GameActionFactory _gameActionFactory;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator ChooseInvestigatorTest()
        {
            _prepareGameUseCase.Execute();
            CardPlace place = _cardsProvider.GetCard<CardPlace>("01111");
            _investigatorsProvider.AllInvestigators.ForEach(investigator =>
            _gameActionFactory.Create(new UpdateStatGameAction(investigator.Turns, GameValues.DEFAULT_TURNS_AMOUNT)).AsCoroutine());

            yield return _gameActionFactory.Create(new MoveCardsGameAction(place, _chapterProvider.CurrentScene.PlaceZone[0, 4])).AsCoroutine();
            if (!DEBUG_MODE) WaitToClickHistoryPanel().AsTask();

            List<Card> allAvatars = _investigatorsProvider.AllInvestigators
               .Select(investigator => investigator.AvatarCard).Cast<Card>().ToList();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(allAvatars, place.OwnZone)).AsCoroutine();


            if (!DEBUG_MODE) WaitToClick(_investigatorsProvider.Leader.AvatarCard).AsTask();
            ChooseInvestigatorGameAction chooseInvestigatoGA = new();
            yield return _gameActionFactory.Create(chooseInvestigatoGA).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(chooseInvestigatoGA.ActiveInvestigator, Is.EqualTo(_investigatorsProvider.Leader));
        }
    }
}
