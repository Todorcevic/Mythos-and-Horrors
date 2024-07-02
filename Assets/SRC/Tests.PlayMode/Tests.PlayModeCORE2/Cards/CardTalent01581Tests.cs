
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardTalent01581Tests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator MoveAndUnconfront()
        {
            Investigator investigator = _investigatorsProvider.Second;
            Card01581 cardTalent = _cardsProvider.GetCard<Card01581>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardTalent, investigator.HandZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE2.Drew, investigator.DangerZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE2.Herman, investigator.DangerZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedClone(SceneCORE2.Drew, 1);
            yield return ClickedIn(cardTalent);
            yield return ClickedMainButton();
            yield return ClickedIn(SceneCORE2.East);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.AllTypeCreaturesConfronted.Any(), Is.False);
            Assert.That(investigator.CurrentPlace, Is.EqualTo(SceneCORE2.East));
            Assert.That(SceneCORE2.Drew.CurrentPlace, Is.EqualTo(SceneCORE2.Fluvial));
            Assert.That(SceneCORE2.Herman.CurrentPlace, Is.EqualTo(SceneCORE2.Fluvial));
            Assert.That(SceneCORE2.Drew.Exausted.IsActive, Is.True);
            Assert.That(SceneCORE2.Herman.Exausted.IsActive, Is.False);
        }
    }
}
