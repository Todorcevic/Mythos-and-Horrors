using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class ChooseInvestigatorGameActionTest : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        //[UnityTest]
        //public IEnumerator ChooseInvestigatorTest()
        //{
        //    yield return _preparationScene.PlayAllInvestigators();
        //    CardPlace place = _cardsProvider.GetCard<Card01111>();

        //    foreach (Investigator investigator in _investigatorsProvider.AllInvestigatorsInPlay)
        //    {
        //        yield return _gameActionsProvider.Create(new UpdateStatGameAction(investigator.CurrentTurns, GameValues.DEFAULT_TURNS_AMOUNT)).AsCoroutine();
        //    }

        //    yield return _gameActionsProvider.Create(new MoveCardsGameAction(place, _chaptersProvider.CurrentScene.PlaceZone[0, 4])).AsCoroutine();
        //    IEnumerable<Card> allAvatars = _investigatorsProvider.AllInvestigatorsInPlay.Select(investigator => investigator.AvatarCard).Cast<Card>();
        //    yield return _gameActionsProvider.Create(new MoveCardsGameAction(allAvatars, place.OwnZone)).AsCoroutine();

        //    if (!DEBUG_MODE) WaitToClick(_investigatorsProvider.First.AvatarCard).AsTask();
        //    ChooseInvestigatorGameAction chooseInvestigatoGA = new(_investigatorsProvider.GetInvestigatorsCanStartTurn);
        //    yield return _gameActionsProvider.Create(chooseInvestigatoGA).AsCoroutine();

        //    if (DEBUG_MODE) yield return new WaitForSeconds(230);
        //    Assert.That(chooseInvestigatoGA.InvestigatorSelected, Is.EqualTo(_investigatorsProvider.First));
        //}
    }
}
