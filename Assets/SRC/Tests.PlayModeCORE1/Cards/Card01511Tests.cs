using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class Card01511Tests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator NoPay()
        {
            CardGoal cardGoal = _cardsProvider.GetCard<Card01110>();
            CardAdversity cardAdversity = _cardsProvider.GetCard<Card01511>();
            yield return _preparationSceneCORE1.PlayAllInvestigators();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationSceneCORE1.SceneCORE1.Hallway, _chaptersProvider.CurrentScene.PlaceZone[0, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationSceneCORE1.SceneCORE1.Parlor, _chaptersProvider.CurrentScene.PlaceZone[1, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.AllInvestigatorsInPlay, _preparationSceneCORE1.SceneCORE1.Hallway)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardGoal, _chaptersProvider.CurrentScene.GoalZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardAdversity, _investigatorsProvider.Third.DangerZone)).AsCoroutine();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationSceneCORE1.SceneCORE1.GhoulPriest, _preparationSceneCORE1.SceneCORE1.Hallway.OwnZone)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new DefeatCardGameAction(_preparationSceneCORE1.SceneCORE1.GhoulPriest, _investigatorsProvider.First.InvestigatorCard));
            if (!DEBUG_MODE) yield return WaitToCloneClick(0);

            yield return taskGameAction.AsCoroutine();

            Assert.That(cardGoal.Revealed.IsActive, Is.True);
            Assert.That(_investigatorsProvider.Third.Xp.Value, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator Pay()
        {
            CardAdversity cardAdversity = _cardsProvider.GetCard<Card01511>();
            Investigator investigator = _investigatorsProvider.Third;
            yield return _preparationSceneCORE1.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardAdversity, investigator.DangerZone)).AsCoroutine();
            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));

            if (!DEBUG_MODE) yield return WaitToClick(cardAdversity);
            if (!DEBUG_MODE) yield return WaitToClick(cardAdversity);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            yield return taskGameAction.AsCoroutine();

            Assert.That(((Card01511)cardAdversity).Resources.Value, Is.EqualTo(4));
        }
    }
}
