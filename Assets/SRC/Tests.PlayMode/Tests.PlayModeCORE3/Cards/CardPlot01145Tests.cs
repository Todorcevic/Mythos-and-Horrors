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
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE3.CurrentPlot, SceneCORE3.OutZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardPlot, SceneCORE3.PlotZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new DecrementStatGameAction(cardPlot.Eldritch, cardPlot.Eldritch.Value)).AsCoroutine();

            yield return _gameActionsProvider.Create(new CheckEldritchsPlotGameAction()).AsCoroutine(); ;

            Assert.That(SceneCORE3.Ritual.IsInPlay, Is.True);
            Assert.That(SceneCORE3.Urmodoth.IsInPlay, Is.True);
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
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE3.CurrentPlot, SceneCORE3.OutZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardPlot, SceneCORE3.PlotZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE3.CurrentGoal, SceneCORE3.OutZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(goal2, SceneCORE3.GoalZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE3.Ritual, SceneCORE3.GetPlaceZone(1, 4))).AsCoroutine();
            yield return _gameActionsProvider.Create(new SpawnCreatureGameAction(creature, SceneCORE3.Ritual)).AsCoroutine();
            yield return _gameActionsProvider.Create(new DecrementStatGameAction(cardPlot.Eldritch, cardPlot.Eldritch.Value)).AsCoroutine();

            yield return _gameActionsProvider.Create(new CheckEldritchsPlotGameAction()).AsCoroutine(); ;

            Assert.That(SceneCORE3.Ritual.IsInPlay, Is.True);
            Assert.That(creature.IsInPlay, Is.False);
            Assert.That(SceneCORE3.Urmodoth.IsInPlay, Is.True);
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
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE3.CurrentPlot, SceneCORE3.OutZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardPlot, SceneCORE3.PlotZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new DecrementStatGameAction(cardPlot.Eldritch, cardPlot.Eldritch.Value)).AsCoroutine();

            yield return _gameActionsProvider.Create(new CheckEldritchsPlotGameAction()).AsCoroutine();
            yield return _gameActionsProvider.Create(new DefeatCardGameAction(SceneCORE3.Urmodoth, investigator2.InvestigatorCard)).AsCoroutine();

            Assert.That(investigator.Injury.Value, Is.EqualTo(2));
            Assert.That(investigator2.Injury.Value, Is.EqualTo(2));
        }
    }
}
