using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardGoal01123Tests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator RevealWhenCultistsDefeat()
        {
            Card01123 goal = _cardsProvider.GetCard<Card01123>();

            yield return StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE2.AllCultists, SceneCORE2.VictoryZone)).AsCoroutine();

            Assert.That(goal.Revealed.IsActive, Is.True);
        }

        [UnityTest]
        public IEnumerator PayHint()
        {
            Card01123 goal = _cardsProvider.GetCard<Card01123>();
            Investigator investigator = _investigatorsProvider.First;
            Investigator investigator2 = _investigatorsProvider.Second;

            yield return StartingScene();
            yield return _gameActionsProvider.Create(new IncrementStatGameAction(SceneCORE2.Fluvial.Hints, 16)).AsCoroutine();
            yield return _gameActionsProvider.Create(new GainHintGameAction(investigator, investigator.CurrentPlace.Hints, 8)).AsCoroutine();
            yield return _gameActionsProvider.Create(new GainHintGameAction(investigator2, investigator2.CurrentPlace.Hints, 8)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(goal);
            yield return ClickAvatarUpDown(_investigatorsProvider.First, isUp: true);
            yield return ClickAvatarUpDown(_investigatorsProvider.First, isUp: true);
            yield return ClickAvatarUpDown(_investigatorsProvider.First, isUp: true);
            yield return ClickAvatarUpDown(_investigatorsProvider.First, isUp: true);
            yield return ClickAvatarUpDown(_investigatorsProvider.Second, isUp: true);
            yield return ClickAvatarUpDown(_investigatorsProvider.Second, isUp: true);
            yield return ClickAvatarUpDown(_investigatorsProvider.Second, isUp: true);
            yield return ClickAvatarUpDown(_investigatorsProvider.Second, isUp: true);
            yield return ClickedMainButonPayHint();
            Assert.That(SceneCORE2.Cultists.Count(cultis => cultis.CurrentZone != SceneCORE2.OutZone), Is.EqualTo(1));
            yield return ClickedIn(goal);
            yield return ClickAvatarUpDown(_investigatorsProvider.First, isUp: true);
            yield return ClickAvatarUpDown(_investigatorsProvider.First, isUp: true);
            yield return ClickAvatarUpDown(_investigatorsProvider.First, isUp: true);
            yield return ClickAvatarUpDown(_investigatorsProvider.First, isUp: true);
            yield return ClickAvatarUpDown(_investigatorsProvider.Second, isUp: true);
            yield return ClickAvatarUpDown(_investigatorsProvider.Second, isUp: true);
            yield return ClickAvatarUpDown(_investigatorsProvider.Second, isUp: true);
            yield return ClickAvatarUpDown(_investigatorsProvider.Second, isUp: true);
            yield return ClickedMainButonPayHint();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(SceneCORE2.Cultists.Count(cultis => cultis.CurrentZone != SceneCORE2.OutZone), Is.EqualTo(2));
        }
    }
}
