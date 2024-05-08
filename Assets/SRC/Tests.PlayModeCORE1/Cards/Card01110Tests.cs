using MythosAndHorrors.GameRules;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class Card01110Tests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator RevealNoBurnTest()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01110>();
            yield return _preparationScene.PlayAllInvestigators();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationScene.SceneCORE1.Hallway, _chaptersProvider.CurrentScene.PlaceZone[0, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationScene.SceneCORE1.Parlor, _chaptersProvider.CurrentScene.PlaceZone[1, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.AllInvestigatorsInPlay, _preparationScene.SceneCORE1.Hallway)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardGoal, _chaptersProvider.CurrentScene.GoalZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationScene.SceneCORE1.GhoulPriest, _preparationScene.SceneCORE1.Hallway.OwnZone)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new DefeatCardGameAction(_preparationScene.SceneCORE1.GhoulPriest, _investigatorsProvider.First.InvestigatorCard));
            if (!DEBUG_MODE) yield return WaitToCloneClick(1);

            yield return taskGameAction.AsCoroutine();

            Assert.That(cardGoal.Revealed.IsActive, Is.True);
            Assert.That(_investigatorsProvider.First.Xp.Value, Is.EqualTo(5));
        }

        [UnityTest]
        public IEnumerator RevealBurnItTest()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01110>();
            yield return _preparationScene.PlayAllInvestigators();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationScene.SceneCORE1.Hallway, _chaptersProvider.CurrentScene.PlaceZone[0, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationScene.SceneCORE1.Parlor, _chaptersProvider.CurrentScene.PlaceZone[1, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.AllInvestigatorsInPlay, _preparationScene.SceneCORE1.Hallway)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardGoal, _chaptersProvider.CurrentScene.GoalZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationScene.SceneCORE1.GhoulPriest, _preparationScene.SceneCORE1.Hallway.OwnZone)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new DefeatCardGameAction(_preparationScene.SceneCORE1.GhoulPriest, _investigatorsProvider.First.InvestigatorCard));
            if (!DEBUG_MODE) yield return WaitToCloneClick(0);

            yield return taskGameAction.AsCoroutine();

            Assert.That(cardGoal.Revealed.IsActive, Is.True);
            Assert.That(_investigatorsProvider.First.Xp.Value, Is.EqualTo(4));
            Assert.That(_investigatorsProvider.First.Shock.Value, Is.EqualTo(1));
        }
    }
}
