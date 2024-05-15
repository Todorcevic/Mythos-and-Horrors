using MythosAndHorrors.GameRules;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class GameActionOpportunityAttackTests : TestCORE1PlayModeBase
    {
        [UnityTest]
        public IEnumerator OpportunityAttackWhenInvestigate()
        {
            CardPlace place = _cardsProvider.GetCard<Card01111>();
            Investigator investigator = _investigatorsProvider.First;
            CardCreature creature = _preparationSceneCORE1.SceneCORE1.GhoulSecuaz;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1);

            yield return _preparationSceneCORE1.PlayThisInvestigator(investigator).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place, _chaptersProvider.CurrentScene.PlaceZone[2, 2])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, place)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, investigator.DangerZone)).AsCoroutine();

            Task<PlayInvestigatorGameAction> gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            FakeInteractablePresenter.ClickedIn(place);
            FakeInteractablePresenter.ClickedIn(investigator.InvestigatorCard);
            FakeInteractablePresenter.ClickedMainButton();
            FakeInteractablePresenter.ClickedMainButton();

            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.DamageRecived, Is.EqualTo(1));
            Assert.That(investigator.FearRecived, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator OpportunityAttackAnyCreatureWithSafeForeachAndEliminate()
        {
            CardPlace place = _cardsProvider.GetCard<Card01111>();
            Investigator investigator = _investigatorsProvider.Fourth;
            CardCreature creature = _preparationSceneCORE1.SceneCORE1.GhoulSecuaz;
            CardCreature creature2 = _preparationSceneCORE1.SceneCORE1.GhoulVoraz;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1);

            yield return _preparationSceneCORE1.PlayThisInvestigator(investigator).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place, _chaptersProvider.CurrentScene.PlaceZone[2, 2])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, place)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, investigator.DangerZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature2, investigator.DangerZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new HarmToCardGameAction(creature2, investigator.InvestigatorCard, amountDamage: 2)).AsCoroutine();

            Task<PlayInvestigatorGameAction> gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            FakeInteractablePresenter.ClickedIn(place);
            FakeInteractablePresenter.ClickedIn(investigator.InvestigatorCard);
            FakeInteractablePresenter.ClickedIn(investigator.InvestigatorCard);
            FakeInteractablePresenter.ClickedIn(creature2);
            FakeInteractablePresenter.ClickedMainButton();
            FakeInteractablePresenter.ClickedMainButton();

            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.DamageRecived, Is.EqualTo(1));
            Assert.That(investigator.FearRecived, Is.EqualTo(1));
        }
    }
}
