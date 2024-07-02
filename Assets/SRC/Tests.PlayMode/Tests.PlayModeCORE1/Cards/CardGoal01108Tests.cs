using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardGoal01108Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator Reveal()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01108>();
            yield return PlayAllInvestigators();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.Study, _chaptersProvider.CurrentScene.GetPlaceZone(0, 3)).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.AllInvestigatorsInPlay, SceneCORE1.Study).Start();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.GhoulSecuaz, SceneCORE1.Study.OwnZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.GhoulVoraz, SceneCORE1.Study.OwnZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardGoal, _chaptersProvider.CurrentScene.GoalZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(cardGoal.Hints, cardGoal.Hints.Value).Start().AsCoroutine();

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
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.GhoulSecuaz, SceneCORE1.Study.OwnZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(SceneCORE1.Study.Hints, cardGoal.Hints.Value).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<GainHintGameAction>().SetWith(_investigatorsProvider.Leader, SceneCORE1.Study.Hints, 5).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<GainHintGameAction>().SetWith(_investigatorsProvider.Second, SceneCORE1.Study.Hints, 3).Start().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(_investigatorsProvider.Leader).Start();
            yield return ClickedIn(cardGoal);
            yield return ClickedIn(_investigatorsProvider.Leader.AvatarCard);
            yield return ClickedIn(_investigatorsProvider.Second.AvatarCard);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(_investigatorsProvider.Leader.Hints.Value, Is.EqualTo(0));
            Assert.That(_investigatorsProvider.Second.Hints.Value, Is.EqualTo(0));
            Assert.That(cardGoal.IsComplete, Is.True);
        }

        [UnityTest]
        public IEnumerator CancelPayHint()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01108>();
            yield return StartingScene();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.GhoulSecuaz, SceneCORE1.Study.OwnZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(SceneCORE1.Study.Hints, cardGoal.Hints.Value).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<GainHintGameAction>().SetWith(_investigatorsProvider.Leader, SceneCORE1.Study.Hints, 5).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<GainHintGameAction>().SetWith(_investigatorsProvider.Second, SceneCORE1.Study.Hints, 3).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<GainHintGameAction>().SetWith(_investigatorsProvider.Third, SceneCORE1.Study.Hints, 1).Start().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(_investigatorsProvider.Leader).Start();
            yield return ClickedTokenButton();
            yield return ClickedIn(cardGoal);
            yield return ClickedIn(_investigatorsProvider.Second.AvatarCard);
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
