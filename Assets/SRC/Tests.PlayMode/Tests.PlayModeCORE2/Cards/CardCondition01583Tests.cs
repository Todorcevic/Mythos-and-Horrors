using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using System.Threading.Tasks;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardCondition01583Tests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator UpdateStatModifierChallenge()
        {
            Investigator investigator = _investigatorsProvider.Fourth;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return BuildCard("01583", investigator);
            Card01583 conditionCard = _cardsProvider.GetCard<Card01583>();
            Card01578 conditionCard2 = _cardsProvider.GetCard<Card01578>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(conditionCard, investigator.HandZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(conditionCard2, investigator.HandZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create(new GainResourceGameAction(investigator, 8)).AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE2.Drew, investigator.DangerZone).Start().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(conditionCard2);
            yield return ClickedIn(conditionCard);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(SceneCORE2.Drew.CurrentZone, Is.EqualTo(SceneCORE2.DangerDeckZone));
            Assert.That(SceneCORE2.Drew.Exausted.IsActive, Is.False);
            Assert.That(SceneCORE2.Drew.FaceDown.IsActive, Is.True);
        }
    }
}
