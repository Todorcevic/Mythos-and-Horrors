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
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE3.CurrentGoal, SceneCORE3.OutZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardGoal, SceneCORE3.GoalZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE3.Ritual, SceneCORE3.GetPlaceZone(1, 4)).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.First, SceneCORE3.Ritual).Execute().AsCoroutine();

            Assert.That(cardGoal.IsComplete, Is.True);
            Assert.That(SceneCORE3.Ritual.CreaturesInThisPlace.Count(), Is.EqualTo(2));
        }
    }
}
