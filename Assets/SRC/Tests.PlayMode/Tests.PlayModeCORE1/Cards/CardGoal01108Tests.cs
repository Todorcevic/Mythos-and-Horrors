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
            yield return StartingScene();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.Study, _chaptersProvider.CurrentScene.GetPlaceZone(0, 3)).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.AllInvestigatorsInPlay, SceneCORE1.Study).Execute();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.GhoulSecuaz, SceneCORE1.Study.OwnZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.GhoulVoraz, SceneCORE1.Study.OwnZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardGoal, _chaptersProvider.CurrentScene.GoalZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(cardGoal.Keys, cardGoal.Keys.Value).Execute().AsCoroutine();

            Assert.That(SceneCORE1.Study.CurrentZone, Is.EqualTo(_chaptersProvider.CurrentScene.OutZone));
            Assert.That(SceneCORE1.GhoulSecuaz.CurrentZone, Is.EqualTo(_chaptersProvider.CurrentScene.DangerDiscardZone));
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay
                .All(investigator => investigator.CurrentPlace == SceneCORE1.Hallway), Is.True);
        }

        [UnityTest]
        public IEnumerator PayKey()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01108>();
            yield return StartingScene();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.GhoulSecuaz, SceneCORE1.Study.OwnZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(SceneCORE1.Study.Keys, cardGoal.Keys.Value).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<GainKeyGameAction>().SetWith(_investigatorsProvider.Leader, SceneCORE1.Study.Keys, 5).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<GainKeyGameAction>().SetWith(_investigatorsProvider.Second, SceneCORE1.Study.Keys, 3).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(_investigatorsProvider.Leader).Execute();
            yield return ClickedIn(cardGoal);
            yield return ClickedIn(_investigatorsProvider.Leader.AvatarCard);
            yield return ClickedIn(_investigatorsProvider.Second.AvatarCard);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(_investigatorsProvider.Leader.Keys.Value, Is.EqualTo(0));
            Assert.That(_investigatorsProvider.Second.Keys.Value, Is.EqualTo(0));
            Assert.That(cardGoal.IsComplete, Is.True);
        }

        //[UnityTest]
        //public IEnumerator CancelPayKey()
        //{
        //    CardGoal cardGoal = _cardsProvider.GetCard<Card01108>();
        //    yield return StartingScene();
        //    yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.GhoulSecuaz, SceneCORE1.Study.OwnZone).Execute().AsCoroutine();
        //    yield return _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(SceneCORE1.Study.Keys, cardGoal.Keys.Value).Execute().AsCoroutine();
        //    yield return _gameActionsProvider.Create<GainKeyGameAction>().SetWith(_investigatorsProvider.Leader, SceneCORE1.Study.Keys, 5).Execute().AsCoroutine();
        //    yield return _gameActionsProvider.Create<GainKeyGameAction>().SetWith(_investigatorsProvider.Second, SceneCORE1.Study.Keys, 3).Execute().AsCoroutine();
        //    yield return _gameActionsProvider.Create<GainKeyGameAction>().SetWith(_investigatorsProvider.Third, SceneCORE1.Study.Keys, 1).Execute().AsCoroutine();

        //    Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(_investigatorsProvider.Leader).Execute();
        //    yield return ClickedResourceButton();
        //    yield return ClickedIn(cardGoal);
        //    yield return ClickedIn(_investigatorsProvider.Second.AvatarCard);
        //    yield return ClickedUndoButton();
        //    yield return ClickedMainButton();
        //    yield return taskGameAction.AsCoroutine();

        //    Assert.That(_investigatorsProvider.Leader.Keys.Value, Is.EqualTo(5));
        //    Assert.That(_investigatorsProvider.Second.Keys.Value, Is.EqualTo(3));
        //    Assert.That(cardGoal.Keys.Value, Is.EqualTo(8));
        //    Assert.That(cardGoal.Revealed.IsActive, Is.False);
        //    Assert.That(_investigatorsProvider.Leader.Resources.Value, Is.EqualTo(1));
        //}
    }
}
