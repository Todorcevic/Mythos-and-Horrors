using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardAdversity01511Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator NoPay()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01110>();
            CardAdversity cardAdversity = _cardsProvider.GetCard<Card01511>();
            yield return PlayAllInvestigators();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.Hallway, _chaptersProvider.CurrentScene.GetPlaceZone(0, 3)).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.Parlor, _chaptersProvider.CurrentScene.GetPlaceZone(1, 3)).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.AllInvestigatorsInPlay, SceneCORE1.Hallway).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardGoal, _chaptersProvider.CurrentScene.GoalZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardAdversity, _investigatorsProvider.Third.DangerZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.GhoulPriest, SceneCORE1.Hallway.OwnZone).Start().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<DefeatCardGameAction>().SetWith(SceneCORE1.GhoulPriest, _investigatorsProvider.First.InvestigatorCard).Start();
            yield return ClickedClone(cardGoal, 0, isReaction: true);
            yield return taskGameAction.AsCoroutine();

            Assert.That(_investigatorsProvider.Third.Xp.Value, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator Pay()
        {
            Card01511 cardAdversity = _cardsProvider.GetCard<Card01511>();
            Investigator investigator = _investigatorsProvider.Third;
            yield return PlayThisInvestigator(investigator, withResources: true);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardAdversity, investigator.DangerZone).Start().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Start();
            yield return ClickedIn(cardAdversity);
            yield return ClickedIn(cardAdversity);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(cardAdversity.Resources.Value, Is.EqualTo(4));
            Assert.That(investigator.Resources.Value, Is.EqualTo(3));
        }
    }
}
