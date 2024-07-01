using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeView.Tests
{
    [TestFixture]
    public class ChangeStatGameActionTests : PlayModeTestsBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Update_Investigator_Stats()
        {
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(_investigatorsProvider.First.InvestigatorCard, _investigatorsProvider.First.InvestigatorZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(_investigatorsProvider.First.Health, 3)).AsCoroutine();
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(_investigatorsProvider.First.CurrentTurns, 2)).AsCoroutine();


            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(_investigatorsProvider.First.Health.Value, Is.EqualTo(3));
            Assert.That((_cardViewsManager.GetCardView(_investigatorsProvider.First.AvatarCard) as AvatarCardView).GetPrivateMember<StatView>("_health").Stat.Value, Is.EqualTo(3));
            Assert.That(_avatarViewsManager.Get(_investigatorsProvider.First).GetPrivateMember<StatUIView>("_healthStat").Stat.Value, Is.EqualTo(3));
            Assert.That(_avatarViewsManager.Get(_investigatorsProvider.First).GetPrivateMember<TurnController>("_turnController").ActiveTurnsCount, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator Move_Resource_From_Card()
        {
            CardSupply cardSupply = _investigatorsProvider.First.FullDeck.OfType<CardSupply>().First();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardSupply, _chaptersProvider.CurrentScene.PlotZone).Start().AsCoroutine();

            do
            {
                yield return _gameActionsProvider.Create(new UpdateStatGameAction(cardSupply.ResourceCost, 8)).AsCoroutine();
                if (DEBUG_MODE) yield return PressAnyKey();
            } while (DEBUG_MODE);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(cardSupply.ResourceCost.Value, Is.EqualTo(8));
            Assert.That((_cardViewsManager.GetCardView(cardSupply) as DeckCardView).GetPrivateMember<StatView>("_cost").Stat.Value, Is.EqualTo(8));
        }

        [UnityTest]
        public IEnumerator Update_Eldritch_Stats()
        {
            CardPlot cardPlot = _chaptersProvider.CurrentScene.PlotCards.First();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardPlot, _chaptersProvider.CurrentScene.PlotZone).Start().AsCoroutine();

            do
            {
                yield return _gameActionsProvider.Create(new UpdateStatGameAction(cardPlot.Eldritch, 2)).AsCoroutine();
                if (DEBUG_MODE) yield return PressAnyKey();

            } while (DEBUG_MODE);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(cardPlot.Eldritch.Value, Is.EqualTo(2));
            Assert.That((_cardViewsManager.GetCardView(cardPlot) as PlotCardView).GetPrivateMember<MultiStatView>("_eldritch").Stat.Value, Is.EqualTo(2));
        }


        [UnityTest]
        public IEnumerator Full_Hint_Stats()
        {
            CardGoal cardGoal = _chaptersProvider.CurrentScene.GoalCards.First();
            CardPlace place = _cardsProvider.GetCard<Card01112>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardGoal, _chaptersProvider.CurrentScene.GoalZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(_investigatorsProvider.First.InvestigatorCard, _investigatorsProvider.First.InvestigatorZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(place, _chaptersProvider.CurrentScene.GetPlaceZone(2, 2)).Start().AsCoroutine();
            yield return _gameActionsProvider.Create(new RevealGameAction(place)).AsCoroutine();

            yield return _gameActionsProvider.Create(new UpdateStatGameAction(place.Hints, 3)).AsCoroutine();
            if (DEBUG_MODE) yield return PressAnyKey();
            yield return _gameActionsProvider.Create(new GainHintGameAction(_investigatorsProvider.First, place.Hints, 2)).AsCoroutine();
            if (DEBUG_MODE) yield return PressAnyKey();
            yield return _gameActionsProvider.Create(new PayHintGameAction(_investigatorsProvider.First, cardGoal.Hints, 1)).AsCoroutine();


            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(cardGoal.Hints.Value, Is.EqualTo(7));
        }

    }
}
