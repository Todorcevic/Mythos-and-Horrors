using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE3.Tests
{
    public class CardPlot01145Tests : TestCORE3Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator RevealInGoal1()
        {
            Card01145 cardPlot = _cardsProvider.GetCard<Card01145>();
            Investigator investigator = _investigatorsProvider.First;
            Investigator investigator2 = _investigatorsProvider.Fourth;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return PlayThisInvestigator(investigator2);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE3.CurrentPlot, SceneCORE3.OutZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardPlot, SceneCORE3.PlotZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(cardPlot.Eldritch, cardPlot.Eldritch.Value).Execute().AsCoroutine();

            yield return _gameActionsProvider.Create<CheckEldritchsPlotGameAction>().Execute().AsCoroutine(); ;

            Assert.That(SceneCORE3.Ritual.IsInPlay.IsTrue, Is.True);
            Assert.That(SceneCORE3.Khargath.IsInPlay.IsTrue, Is.True);
        }

        [UnityTest]
        public IEnumerator RevealInGoal2()
        {
            Card01145 cardPlot = _cardsProvider.GetCard<Card01145>();
            Card01147 goal2 = _cardsProvider.GetCard<Card01147>();
            CardCreature creature = _cardsProvider.GetCard<Card01169>();
            Investigator investigator = _investigatorsProvider.First;
            Investigator investigator2 = _investigatorsProvider.Fourth;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return PlayThisInvestigator(investigator2);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE3.CurrentPlot, SceneCORE3.OutZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardPlot, SceneCORE3.PlotZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE3.CurrentGoal, SceneCORE3.OutZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(goal2, SceneCORE3.GoalZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE3.Ritual, SceneCORE3.GetPlaceZone(1, 4)).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(creature, SceneCORE3.Ritual).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(cardPlot.Eldritch, cardPlot.Eldritch.Value).Execute().AsCoroutine();

            yield return _gameActionsProvider.Create<CheckEldritchsPlotGameAction>().Execute().AsCoroutine(); ;

            Assert.That(SceneCORE3.Ritual.IsInPlay.IsTrue, Is.True);
            Assert.That(creature.IsInPlay.IsTrue, Is.False);
            Assert.That(SceneCORE3.Khargath.IsInPlay.IsTrue, Is.True);
        }

        [UnityTest]
        public IEnumerator UrmodothDefeated()
        {
            Card01145 cardPlot = _cardsProvider.GetCard<Card01145>();
            Investigator investigator = _investigatorsProvider.First;
            Investigator investigator2 = _investigatorsProvider.Fourth;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return PlayThisInvestigator(investigator2);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE3.CurrentPlot, SceneCORE3.OutZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardPlot, SceneCORE3.PlotZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(cardPlot.Eldritch, cardPlot.Eldritch.Value).Execute().AsCoroutine();

            yield return _gameActionsProvider.Create<CheckEldritchsPlotGameAction>().Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<DefeatCardGameAction>().SetWith(SceneCORE3.Khargath, investigator2.InvestigatorCard).Execute().AsCoroutine();

            Assert.That(investigator.Injury.Value, Is.EqualTo(2));
            Assert.That(investigator2.Injury.Value, Is.EqualTo(2));
        }
    }
}
