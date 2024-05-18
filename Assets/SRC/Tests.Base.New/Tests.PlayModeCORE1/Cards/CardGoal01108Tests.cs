using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class CardGoal01108Tests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator Reveal()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01108>();
            yield return PlayAllInvestigators();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE1.Study, _chaptersProvider.CurrentScene.PlaceZone[0, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.AllInvestigatorsInPlay, SceneCORE1.Study));
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE1.GhoulSecuaz, SceneCORE1.Study.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE1.GhoulVoraz, SceneCORE1.Study.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardGoal, _chaptersProvider.CurrentScene.GoalZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new DecrementStatGameAction(cardGoal.Hints, cardGoal.Hints.Value)).AsCoroutine();

            Assert.That(SceneCORE1.Study.CurrentZone, Is.EqualTo(_chaptersProvider.CurrentScene.OutZone));
            Assert.That(SceneCORE1.GhoulSecuaz.CurrentZone, Is.EqualTo(_chaptersProvider.CurrentScene.DangerDiscardZone));
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay
                .All(investigator => investigator.CurrentPlace == SceneCORE1.Hallway), Is.True);
        }

        [UnityTest]
        public IEnumerator PayHint()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01108>();
            yield return StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE1.GhoulSecuaz, SceneCORE1.Study.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new IncrementStatGameAction(SceneCORE1.Study.Hints, cardGoal.Hints.Value)).AsCoroutine();
            yield return _gameActionsProvider.Create(new GainHintGameAction(_investigatorsProvider.Leader, SceneCORE1.Study.Hints, 5)).AsCoroutine();
            yield return _gameActionsProvider.Create(new GainHintGameAction(_investigatorsProvider.Second, SceneCORE1.Study.Hints, 3)).AsCoroutine();

            Task<PlayInvestigatorGameAction> taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.Leader));
            yield return ClickedIn(cardGoal);
            yield return ClickedIn(_investigatorsProvider.Leader.AvatarCard);
            yield return ClickedIn(_investigatorsProvider.Second.AvatarCard);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(_investigatorsProvider.Leader.Hints.Value, Is.EqualTo(0));
            Assert.That(_investigatorsProvider.Second.Hints.Value, Is.EqualTo(0));
            Assert.That(cardGoal.Hints.Value, Is.EqualTo(0));
            Assert.That(cardGoal.Revealed.IsActive, Is.True);
        }

        [UnityTest]
        public IEnumerator CancelPayHint()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01108>();
            yield return StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE1.GhoulSecuaz, SceneCORE1.Study.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new IncrementStatGameAction(SceneCORE1.Study.Hints, cardGoal.Hints.Value)).AsCoroutine();
            yield return _gameActionsProvider.Create(new GainHintGameAction(_investigatorsProvider.Leader, SceneCORE1.Study.Hints, 5)).AsCoroutine();
            yield return _gameActionsProvider.Create(new GainHintGameAction(_investigatorsProvider.Second, SceneCORE1.Study.Hints, 3)).AsCoroutine();

            Task<PlayInvestigatorGameAction> taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.Leader));
            yield return ClickedTokenButton();
            yield return ClickedIn(_investigatorsProvider.Leader.InvestigatorCard);
            yield return ClickedIn(cardGoal);
            yield return ClickedIn(_investigatorsProvider.Second.AvatarCard);
            yield return ClickedUndoButton();
            yield return ClickedUndoButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(_investigatorsProvider.Leader.Hints.Value, Is.EqualTo(5));
            Assert.That(_investigatorsProvider.Second.Hints.Value, Is.EqualTo(3));
            Assert.That(cardGoal.Hints.Value, Is.EqualTo(8));
            Assert.That(cardGoal.Revealed.IsActive, Is.False);
            Assert.That(_investigatorsProvider.Leader.Resources.Value, Is.EqualTo(1));
        }
    }
}
