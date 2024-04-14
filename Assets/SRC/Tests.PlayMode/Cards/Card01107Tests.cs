using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class Card01107Tests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator GotoResolution3()
        {
            CardPlot cardPlot = _cardsProvider.GetCard<Card01107>();
            CardGoal cardGoal = _cardsProvider.GetCard<Card01109>();
            yield return _preparationScene.PlayAllInvestigators();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardPlot, _chaptersProvider.CurrentScene.PlotZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardGoal, _chaptersProvider.CurrentScene.GoalZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new DecrementStatGameAction(cardPlot.Eldritch, cardPlot.Eldritch.Value)).AsCoroutine();
            yield return _gameActionsProvider.Create(new CheckEldritchsPlotGameAction()).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.LitaGoAway), Is.True);
        }

        [UnityTest]
        public IEnumerator SufferInjury()
        {
            CardPlot cardPlot = _cardsProvider.GetCard<Card01107>();
            CardGoal cardGoal = _cardsProvider.GetCard<Card01110>();
            yield return _preparationScene.PlayAllInvestigators();
            yield return _gameActionsProvider.Create(new UpdateStatesGameAction(_investigatorsProvider.Second.Resign, true)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardPlot, _chaptersProvider.CurrentScene.PlotZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardGoal, _chaptersProvider.CurrentScene.GoalZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new DecrementStatGameAction(cardPlot.Eldritch, cardPlot.Eldritch.Value)).AsCoroutine();
            yield return _gameActionsProvider.Create(new CheckEldritchsPlotGameAction()).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_investigatorsProvider.Leader.Injury.Value, Is.EqualTo(1));
            Assert.That(_investigatorsProvider.Second.Injury.Value, Is.EqualTo(0));
        }

        [UnityTest]
        public IEnumerator MoveGhouls()
        {
            CardPlace place2 = _cardsProvider.GetCard<Card01112>();
            CardPlace place3 = _cardsProvider.GetCard<Card01113>();
            CardPlace place4 = _cardsProvider.GetCard<Card01114>();
            CardCreature ghoul = _cardsProvider.GetCard<Card01119>();
            CardCreature noGhoul = _cardsProvider.GetCard<Card01603>();
            CardPlot cardPlot = _cardsProvider.GetCard<Card01107>();
            yield return _preparationScene.PlayAllInvestigators();
            yield return _preparationScene.PlaceAllPlaceCards();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(ghoul, place4.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(noGhoul, place3.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardPlot, _chaptersProvider.CurrentScene.PlotZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new CreaturePhaseGameAction()).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(ghoul.CurrentPlace, Is.EqualTo(place2));
            Assert.That(noGhoul.CurrentPlace, Is.EqualTo(place3));
        }

        [UnityTest]
        public IEnumerator PlaceEldritch()
        {
            CardPlace place = _cardsProvider.GetCard<Card01112>();
            CardCreature ghoul = _cardsProvider.GetCard<Card01119>();
            CardCreature noGhoul = _cardsProvider.GetCard<Card01603>();
            CardPlot cardPlot = _cardsProvider.GetCard<Card01107>();
            yield return _preparationScene.PlayAllInvestigators();
            yield return _preparationScene.PlaceAllPlaceCards();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(ghoul, place.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(noGhoul, place.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardPlot, _chaptersProvider.CurrentScene.PlotZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new RestorePhaseGameAction()).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(cardPlot.Eldritch.Value, Is.EqualTo(cardPlot.Info.Eldritch - 1));
        }
    }
}
