
using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE3.Tests
{
    public class CardPlot01144Tests : TestCORE3Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator ChallengePowerWhenComplete()
        {
            Card01144 cardPlot = _cardsProvider.GetCard<Card01144>();
            Investigator investigator = _investigatorsProvider.First;
            Investigator investigator2 = _investigatorsProvider.Fourth;
            MustBeRevealedThisToken(ChallengeTokenType.Value0).ContinueWith((_) => MustBeRevealedThisToken(ChallengeTokenType.Value1));
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return PlayThisInvestigator(investigator2);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE3.CurrentPlot, SceneCORE3.OutZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardPlot, SceneCORE3.PlotZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(cardPlot.Eldritch, cardPlot.Eldritch.Value).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<CheckEldritchsPlotGameAction>().Execute();
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.DangerZone.Cards.Any(card => card is Card01598), Is.True);
            Assert.That(investigator2.DangerZone.Cards.Any(card => card is Card01598), Is.False);
        }

        [UnityTest]
        public IEnumerator BuffMonsters()
        {
            Card01144 cardPlot = _cardsProvider.GetCard<Card01144>();
            CardCreature creature = _cardsProvider.GetCard<Card01169>();
            yield return StartingScene();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE3.CurrentPlot, SceneCORE3.OutZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardPlot, SceneCORE3.PlotZone).Execute().AsCoroutine();

            yield return _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(creature, SceneCORE3.MainPath).Execute().AsCoroutine();

            Assert.That(creature.Strength.Value, Is.EqualTo(creature.Info.Strength + 1));
            Assert.That(creature.Agility.Value, Is.EqualTo(creature.Info.Agility + 1));
        }
    }
}
