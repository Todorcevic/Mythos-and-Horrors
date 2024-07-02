using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE3.Tests
{
    public class CardGoal01148Tests : TestCORE3Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator PayHintsWithChallenge()
        {
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1).ContinueWith((_) => MustBeRevealedThisToken(ChallengeTokenType.Value_2));
            Investigator investigator = _investigatorsProvider.First;
            CardGoal cardGoal = _cardsProvider.GetCard<Card01148>();
            yield return StartingScene();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE3.CurrentGoal, SceneCORE3.OutZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardGoal, SceneCORE3.GoalZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<GainHintGameAction>().SetWith(investigator, SceneCORE3.Forests[0].Hints, 2).Execute().AsCoroutine();
            int actualHintsInCurrentPlace = investigator.CurrentPlace.Hints.Value;
            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(cardGoal);
            yield return ClickedMainButton();
            Assert.That(cardGoal.Hints.Value, Is.EqualTo(7));
            yield return ClickedIn(cardGoal);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();
            Assert.That(cardGoal.Hints.Value, Is.EqualTo(7));
            Assert.That(investigator.CurrentPlace.Hints.Value, Is.EqualTo(actualHintsInCurrentPlace + 1));
        }

        [UnityTest]
        public IEnumerator CompleteEffect()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01148>();
            yield return StartingScene();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE3.CurrentGoal, SceneCORE3.OutZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardGoal, SceneCORE3.GoalZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(SceneCORE3.CurrentGoal.Hints, 0).Execute().AsCoroutine();

            Assert.That(_investigatorsProvider.First.Shock.Value, Is.EqualTo(2));
            Assert.That(cardGoal.IsComplete, Is.True);
        }
    }
}
