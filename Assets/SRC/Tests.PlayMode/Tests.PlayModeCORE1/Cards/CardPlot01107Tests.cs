﻿using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardPlot01107Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator GotoResolution3()
        {
            CardPlot cardPlot = _cardsProvider.GetCard<Card01107>();
            CardGoal cardGoal = _cardsProvider.GetCard<Card01109>();
            yield return PlayAllInvestigators();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardPlot, _chaptersProvider.CurrentScene.PlotZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardGoal, _chaptersProvider.CurrentScene.GoalZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(cardPlot.Eldritch, cardPlot.Eldritch.Value).Execute().AsCoroutine();

            yield return _gameActionsProvider.Create<CheckEldritchsPlotGameAction>().Execute().AsCoroutine();

            Assert.That(_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.LitaGoAway), Is.True);
            Assert.That(_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.HouseUp), Is.True);
            Assert.That(_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.PriestGhoulLive), Is.True);
        }

        [UnityTest]
        public IEnumerator SufferInjury()
        {
            CardPlot cardPlot = _cardsProvider.GetCard<Card01107>();
            CardGoal cardGoal = _cardsProvider.GetCard<Card01110>();
            yield return PlayAllInvestigators();
            yield return _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(_investigatorsProvider.Second.Resign, true).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardPlot, _chaptersProvider.CurrentScene.PlotZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardGoal, _chaptersProvider.CurrentScene.GoalZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(cardPlot.Eldritch, cardPlot.Eldritch.Value).Execute().AsCoroutine();

            yield return _gameActionsProvider.Create<CheckEldritchsPlotGameAction>().Execute().AsCoroutine();

            Assert.That(_investigatorsProvider.Leader.Injury.Value, Is.EqualTo(1));
            Assert.That(_investigatorsProvider.Second.Injury.Value, Is.EqualTo(0));
        }

        [UnityTest]
        public IEnumerator MoveGhoulsAtEndCreaturePhace()
        {
            CardPlace place2 = _cardsProvider.GetCard<Card01112>();
            CardPlace place3 = _cardsProvider.GetCard<Card01113>();
            CardPlace place4 = _cardsProvider.GetCard<Card01114>();
            CardPlace place5 = _cardsProvider.GetCard<Card01115>();
            CardCreature ghoul = _cardsProvider.GetCard<Card01119>();
            CardCreature noGhoul = _cardsProvider.GetCard<Card01603>();
            CardPlot cardPlot = _cardsProvider.GetCard<Card01107>();
            yield return PlaceOnlyScene();

            yield return _gameActionsProvider.Create<RevealGameAction>().SetWith(place5).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(ghoul, place4.OwnZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(noGhoul, place3.OwnZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardPlot, _chaptersProvider.CurrentScene.PlotZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<CreaturePhaseGameAction>().Execute().AsCoroutine();

            Assert.That(ghoul.CurrentPlace, Is.EqualTo(place2));
            Assert.That(noGhoul.CurrentPlace, Is.EqualTo(place3));
        }

        [UnityTest]
        public IEnumerator PlaceExtraEldritchAtEndRound()
        {
            CardPlace place = _cardsProvider.GetCard<Card01112>();
            CardCreature ghoul = _cardsProvider.GetCard<Card01119>();
            CardCreature noGhoul = _cardsProvider.GetCard<Card01603>();
            CardPlot cardPlot = _cardsProvider.GetCard<Card01107>();
            Card01506 cardToDraw = _cardsProvider.GetCard<Card01506>();
            Investigator investigator = _investigatorsProvider.First;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(ghoul, place.OwnZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(noGhoul, place.OwnZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardToDraw, investigator.DeckZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardPlot, _chaptersProvider.CurrentScene.PlotZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<RoundGameAction>().Execute();
            yield return ClickedIn(investigator.AvatarCard);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(cardPlot.AmountDecrementedEldritch, Is.EqualTo(1));
        }
    }
}
