using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{

    public class Card01108Tests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator RevealTest()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01108>();
            yield return _preparationSceneCORE1.PlayAllInvestigators();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationSceneCORE1.SceneCORE1.Study, _chaptersProvider.CurrentScene.PlaceZone[0, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.AllInvestigatorsInPlay, _preparationSceneCORE1.SceneCORE1.Study));
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationSceneCORE1.SceneCORE1.GhoulSecuaz, _preparationSceneCORE1.SceneCORE1.Study.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationSceneCORE1.SceneCORE1.GhoulVoraz, _preparationSceneCORE1.SceneCORE1.Study.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardGoal, _chaptersProvider.CurrentScene.GoalZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new DecrementStatGameAction(cardGoal.Hints, cardGoal.Hints.Value)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_preparationSceneCORE1.SceneCORE1.Study.CurrentZone, Is.EqualTo(_chaptersProvider.CurrentScene.OutZone));
            Assert.That(_preparationSceneCORE1.SceneCORE1.GhoulSecuaz.CurrentZone, Is.EqualTo(_chaptersProvider.CurrentScene.DangerDiscardZone));
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay
                .All(investigator => investigator.CurrentPlace == _preparationSceneCORE1.SceneCORE1.Hallway), Is.True);
        }

        [UnityTest]
        public IEnumerator PayHintTest()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01108>();
            yield return _preparationSceneCORE1.StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationSceneCORE1.SceneCORE1.GhoulSecuaz, _preparationSceneCORE1.SceneCORE1.Study.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new IncrementStatGameAction(_preparationSceneCORE1.SceneCORE1.Study.Hints, cardGoal.Hints.Value)).AsCoroutine();
            yield return _gameActionsProvider.Create(new GainHintGameAction(_investigatorsProvider.Leader, _preparationSceneCORE1.SceneCORE1.Study.Hints, 5)).AsCoroutine();
            yield return _gameActionsProvider.Create(new GainHintGameAction(_investigatorsProvider.Second, _preparationSceneCORE1.SceneCORE1.Study.Hints, 3)).AsCoroutine();

            Task<PlayInvestigatorGameAction> taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.Leader));
            if (!DEBUG_MODE) yield return WaitToClick(cardGoal);
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.Leader.AvatarCard);
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.Second.AvatarCard);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            yield return taskGameAction.AsCoroutine();

            Assert.That(_investigatorsProvider.Leader.Hints.Value, Is.EqualTo(0));
            Assert.That(_investigatorsProvider.Second.Hints.Value, Is.EqualTo(0));
            Assert.That(cardGoal.Hints.Value, Is.EqualTo(0));
            Assert.That(cardGoal.Revealed.IsActive, Is.True);
        }

        [UnityTest]
        public IEnumerator CancelPayHintTest()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01108>();
            yield return _preparationSceneCORE1.StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationSceneCORE1.SceneCORE1.GhoulSecuaz, _preparationSceneCORE1.SceneCORE1.Study.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new IncrementStatGameAction(_preparationSceneCORE1.SceneCORE1.Study.Hints, cardGoal.Hints.Value)).AsCoroutine();
            yield return _gameActionsProvider.Create(new GainHintGameAction(_investigatorsProvider.Leader, _preparationSceneCORE1.SceneCORE1.Study.Hints, 5)).AsCoroutine();
            yield return _gameActionsProvider.Create(new GainHintGameAction(_investigatorsProvider.Second, _preparationSceneCORE1.SceneCORE1.Study.Hints, 3)).AsCoroutine();

            Task<PlayInvestigatorGameAction> taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.Leader));
            if (!DEBUG_MODE) yield return WaitToTokenClick();
            if (!DEBUG_MODE) yield return WaitToClick(cardGoal);
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.Second.AvatarCard);
            if (!DEBUG_MODE) yield return WaitToUndoClick();
            if (!DEBUG_MODE) yield return WaitToUndoClick();
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            yield return taskGameAction.AsCoroutine();
            Assert.That(_investigatorsProvider.Leader.Hints.Value, Is.EqualTo(5));
            Assert.That(_investigatorsProvider.Second.Hints.Value, Is.EqualTo(3));
            Assert.That(cardGoal.Hints.Value, Is.EqualTo(8));
            Assert.That(cardGoal.Revealed.IsActive, Is.False);
            Assert.That(_investigatorsProvider.Leader.Resources.Value, Is.EqualTo(1));
        }
    }
}
