using MythosAndHorrors.GameRules;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using System.Linq;
using System;
using Zenject;
using System.Threading.Tasks;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class PrepareSceneCORE3OOOOOOOTests : TestCORE1PlayModeBase
    {
        [Inject] private readonly IInteractablePresenter _interactablePresenter;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator PrepareSceneOOOOTest()
        {
            SceneCORE1 scene = _preparationSceneCORE1.SceneCORE1;
            yield return _preparationSceneCORE1.PlayAllInvestigators(withAvatar: false);

            yield return _gameActionsProvider.Create(new PrepareSceneGameAction(scene)).AsCoroutine();

            Assert.That(scene.Info.PlaceCards.Where(place => place.IsInPlay).Count(), Is.EqualTo(1));
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.Select(investigator => investigator.CurrentPlace).Unique(),
           Is.EqualTo(scene.Study));
        }

        [UnityTest]
        public IEnumerator Investigator1DiscoverClueOOOOOOOO()
        {
            CardInvestigator cardInvestigator = _cardsProvider.GetCard<Card01501>();
            Investigator investigatorToTest = cardInvestigator.Owner;
            CardCreature cardCreature = _preparationSceneCORE1.SceneCORE1.GhoulSecuaz;
            yield return _preparationSceneCORE1.StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardCreature, investigatorToTest.DangerZone)).AsCoroutine();
            Task taskGameAction = _gameActionsProvider.Create(new HarmToCardGameAction(cardCreature, investigatorToTest.InvestigatorCard, amountDamage: 5));

            FakeInteractablePresenter.ClickedIn(investigatorToTest.InvestigatorCard);
            yield return taskGameAction.AsCoroutine();
            Assert.That(investigatorToTest.Hints.Value, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator InvestigatePlaceOOO()
        {
            MustBeRevealedThisToken(ChallengeTokenType.Value_1);
            yield return _preparationSceneCORE1.PlayThisInvestigator(_investigatorsProvider.First);
            CardPlace place = _cardsProvider.GetCard<Card01113>();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place, _chaptersProvider.CurrentScene.PlaceZone[2, 2])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.Leader, place)).AsCoroutine();

            Task<PlayInvestigatorGameAction> gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.First));
            FakeInteractablePresenter.ClickedIn(place);
            FakeInteractablePresenter.ClickMainButton();
            FakeInteractablePresenter.ClickMainButton();

            yield return gameActionTask.AsCoroutine();

            Assert.That(_investigatorsProvider.First.Hints.Value, Is.EqualTo(1));
        }
    }
}
