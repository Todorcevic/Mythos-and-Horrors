using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    [TestFixture]
    public class ChangeStatGameActionTests : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUse;
        [Inject] private readonly GameActionProvider _gameActionFactory;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;
        [Inject] private readonly CardsProvider _cardsProvider;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Update_Investigator_Stats()
        {
            _prepareGameUse.Execute();

            yield return _gameActionFactory.Create(new MoveCardsGameAction(_investigatorsProvider.Leader.InvestigatorCard, _investigatorsProvider.Leader.InvestigatorZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new UpdateStatGameAction(_investigatorsProvider.Leader.Health, 3)).AsCoroutine();
            yield return _gameActionFactory.Create(new UpdateStatGameAction(_investigatorsProvider.Leader.CurrentTurns, 2)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(_investigatorsProvider.Leader.Health.Value, Is.EqualTo(3));
            Assert.That((_cardViewsManager.GetCardView(_investigatorsProvider.Leader.AvatarCard) as AvatarCardView).GetPrivateMember<StatView>("_health").Stat.Value, Is.EqualTo(3));
            Assert.That(_avatarViewsManager.Get(_investigatorsProvider.Leader).GetPrivateMember<StatUIView>("_healthStat").Stat.Value, Is.EqualTo(3));
            Assert.That(_avatarViewsManager.Get(_investigatorsProvider.Leader).GetPrivateMember<TurnController>("_turnController").ActiveTurnsCount, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator Move_Resource_From_Card()
        {
            _prepareGameUse.Execute();
            CardSupply cardSupply = _investigatorsProvider.Leader.Cards[0] as CardSupply;
            yield return _gameActionFactory.Create(new MoveCardsGameAction(cardSupply, _chaptersProvider.CurrentScene.PlotZone)).AsCoroutine();

            do
            {
                yield return _gameActionFactory.Create(new UpdateStatGameAction(cardSupply.ResourceCost, 8)).AsCoroutine();
                if (DEBUG_MODE) yield return PressAnyKey();
            } while (DEBUG_MODE);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(cardSupply.ResourceCost.Value, Is.EqualTo(8));
            Assert.That((_cardViewsManager.GetCardView(cardSupply) as DeckCardView).GetPrivateMember<StatView>("_cost").Stat.Value, Is.EqualTo(8));
        }

        [UnityTest]
        public IEnumerator Update_Eldritch_Stats()
        {
            _prepareGameUse.Execute();
            CardPlot cardPlot = _chaptersProvider.CurrentScene.Info.PlotCards.First();
            if (!DEBUG_MODE) WaitToHistoryPanelClick().AsTask();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(cardPlot, _chaptersProvider.CurrentScene.PlotZone)).AsCoroutine();

            do
            {
                yield return _gameActionFactory.Create(new UpdateStatGameAction(cardPlot.Eldritch, 2)).AsCoroutine();
                if (DEBUG_MODE) yield return PressAnyKey();

            } while (DEBUG_MODE);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(cardPlot.Eldritch.Value, Is.EqualTo(2));
            Assert.That((_cardViewsManager.GetCardView(cardPlot) as PlotCardView).GetPrivateMember<StatView>("_eldritch").Stat.Value, Is.EqualTo(2));
        }


        [UnityTest]
        public IEnumerator Full_Hint_Stats()
        {
            _prepareGameUse.Execute();
            CardGoal cardGoal = _chaptersProvider.CurrentScene.Info.GoalCards.First();
            CardPlace place = _cardsProvider.GetCard<CardPlace>("01112");
            yield return _gameActionFactory.Create(new MoveCardsGameAction(cardGoal, _chaptersProvider.CurrentScene.GoalZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(_investigatorsProvider.Leader.InvestigatorCard, _investigatorsProvider.Leader.InvestigatorZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(place, _chaptersProvider.CurrentScene.PlaceZone[2, 2])).AsCoroutine();
            if (!DEBUG_MODE) WaitToHistoryPanelClick().AsTask();
            yield return _gameActionFactory.Create(new RevealGameAction(place)).AsCoroutine();

            yield return _gameActionFactory.Create(new UpdateStatGameAction(place.Hints, 3)).AsCoroutine();
            if (DEBUG_MODE) yield return PressAnyKey();
            yield return _gameActionFactory.Create(new GainHintGameAction(_investigatorsProvider.Leader, place.Hints, 2)).AsCoroutine();
            if (DEBUG_MODE) yield return PressAnyKey();
            yield return _gameActionFactory.Create(new PayHintGameAction(_investigatorsProvider.Leader, cardGoal.Hints, 1)).AsCoroutine();


            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(cardGoal.Hints.Value, Is.EqualTo(1));
        }

    }
}
