using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE3.Tests
{
    public class CardGoal01147Tests : TestCORE3Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator CompleteEffect()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01147>();
            yield return StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE3.CurrentGoal, SceneCORE3.OutZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardGoal, SceneCORE3.GoalZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE3.Ritual, SceneCORE3.PlaceZone[1, 4])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.First, SceneCORE3.Ritual)).AsCoroutine();

            Assert.That(cardGoal.Revealed.IsActive, Is.True);
            Assert.That(SceneCORE3.Ritual.CreaturesInThisPlace.Count(), Is.EqualTo(2));
        }
    }
}
