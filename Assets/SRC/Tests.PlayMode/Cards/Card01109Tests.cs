using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class Card01109Tests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator RevealTest()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01109>();
            yield return _preparationScene.PlayAllInvestigators();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationScene.Hallway, _chaptersProvider.CurrentScene.PlaceZone[0, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationScene.Parlor, _chaptersProvider.CurrentScene.PlaceZone[1, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.AllInvestigatorsInPlay, _preparationScene.Hallway)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardGoal, _chaptersProvider.CurrentScene.GoalZone)).AsCoroutine();

            yield return _gameActionsProvider.Create(new DecrementStatGameAction(cardGoal.Hints, cardGoal.Hints.Value)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_preparationScene.Parlor.Revealed.IsActive, Is.True);
            Assert.That(cardGoal.Revealed.IsActive, Is.True);
            Assert.That(_preparationScene.Lita.CurrentZone, Is.EqualTo(_preparationScene.Parlor.OwnZone));
            Assert.That(_preparationScene.GhoulPriest.CurrentZone, Is.EqualTo(_preparationScene.Hallway.OwnZone));
        }

        [UnityTest]
        public IEnumerator PayHintTest()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01109>();
            yield return _preparationScene.PlayLeadInvestigator();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationScene.Hallway, _chaptersProvider.CurrentScene.PlaceZone[0, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationScene.Parlor, _chaptersProvider.CurrentScene.PlaceZone[1, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.Leader, _preparationScene.Hallway));
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardGoal, _chaptersProvider.CurrentScene.GoalZone)).AsCoroutine();

            yield return _gameActionsProvider.Create(new IncrementStatGameAction(_preparationScene.Hallway.Hints, cardGoal.Hints.Value)).AsCoroutine();
            yield return _gameActionsProvider.Create(new GainHintGameAction(_investigatorsProvider.Leader, _preparationScene.Hallway.Hints, 12)).AsCoroutine();

            Task<RoundGameAction> taskGameAction = _gameActionsProvider.Create(new RoundGameAction());
            if (!DEBUG_MODE) yield return WaitToTokenClick();
            if (!DEBUG_MODE) yield return WaitToTokenClick();
            if (!DEBUG_MODE) yield return WaitToTokenClick();

            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.Leader.AvatarCard);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            while (!taskGameAction.IsCompleted) yield return null;

            Assert.That(_investigatorsProvider.Leader.Hints.Value, Is.EqualTo(0));
            Assert.That(_investigatorsProvider.Second.Hints.Value, Is.EqualTo(0));
            Assert.That(cardGoal.Hints.Value, Is.EqualTo(0));
            Assert.That(cardGoal.Revealed.IsActive, Is.True);
        }
    }
}
