//using MythosAndHorrors.GameRules;
//using MythosAndHorrors.PlayMode.Tests;
//using NUnit.Framework;
//using System.Collections;
//using System.Threading.Tasks;
//using UnityEngine.TestTools;

//namespace MythosAndHorrors.PlayModeCORE1.Tests
//{
//    public class CardAdversity01164Tests : TestCORE1Preparation
//    {
//        //protected override TestsType TestsType => TestsType.Debug;

//        [UnityTest]
//        public IEnumerator MoveCostExtraTurn()
//        {
//            Investigator investigator = _investigatorsProvider.First;
//            Card01164 cardAdversity = _cardsProvider.GetCard<Card01164>();
//            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_1);
//            yield return PlaceOnlyScene();
//            yield return PlayThisInvestigator(investigator);
//            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, SceneCORE1.Hallway).Execute().AsCoroutine();
//            yield return _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, cardAdversity).Execute().AsCoroutine();
//            Assert.That(cardAdversity.Wasted.IsActive, Is.False);

//            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
//            yield return ClickedIn(SceneCORE1.Attic);
//            yield return ClickedIn(SceneCORE1.Hallway);
//            yield return AssertThatIsNotClickable(SceneCORE1.Attic);
//            yield return ClickedMainButton();
//            yield return ClickedMainButton();
//            yield return taskGameAction.AsCoroutine();

//            Assert.That(cardAdversity.CurrentZone, Is.EqualTo(investigator.DangerZone));
//            Assert.That(cardAdversity.Wasted.IsActive, Is.False);
//        }

//        [UnityTest]
//        public IEnumerator EludeCostExtraTurn()
//        {
//            Investigator investigator = _investigatorsProvider.First;
//            Card01164 cardAdversity = _cardsProvider.GetCard<Card01164>();
//            _ = MustBeRevealedThisToken(ChallengeTokenType.Fail).ContinueWith((_) => MustBeRevealedThisToken(ChallengeTokenType.Value1));
//            yield return PlaceOnlyScene();
//            yield return PlayThisInvestigator(investigator);
//            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, SceneCORE1.Hallway).Execute().AsCoroutine();
//            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.GhoulSecuaz, investigator.DangerZone).Execute().AsCoroutine();
//            yield return _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, cardAdversity).Execute().AsCoroutine();

//            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
//            yield return ClickedClone(SceneCORE1.GhoulSecuaz, 1);
//            yield return ClickedMainButton();
//            yield return ClickedResourceButton();
//            yield return AssertThatIsNotClickable(SceneCORE1.Attic);
//            yield return ClickedMainButton();
//            yield return ClickedMainButton();
//            yield return taskGameAction.AsCoroutine();

//            Assert.That(cardAdversity.CurrentZone, Is.EqualTo(SceneCORE1.DangerDiscardZone));
//            Assert.That(cardAdversity.Wasted.IsActive, Is.False);
//        }

//        [UnityTest]
//        public IEnumerator AttackCostExtraTurn()
//        {
//            Investigator investigator = _investigatorsProvider.First;
//            Card01164 cardAdversity = _cardsProvider.GetCard<Card01164>();
//            _ = MustBeRevealedThisToken(ChallengeTokenType.Fail).ContinueWith((_) => MustBeRevealedThisToken(ChallengeTokenType.Value1));
//            yield return PlaceOnlyScene();
//            yield return PlayThisInvestigator(investigator);
//            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, SceneCORE1.Hallway).Execute().AsCoroutine();
//            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.GhoulSecuaz, investigator.DangerZone).Execute().AsCoroutine();
//            yield return _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, cardAdversity).Execute().AsCoroutine();

//            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
//            yield return ClickedClone(SceneCORE1.GhoulSecuaz, 0);
//            yield return ClickedMainButton();
//            yield return ClickedResourceButton();
//            yield return AssertThatIsNotClickable(SceneCORE1.Attic);
//            yield return ClickedMainButton();
//            yield return ClickedMainButton();
//            yield return taskGameAction.AsCoroutine();

//            Assert.That(cardAdversity.CurrentZone, Is.EqualTo(SceneCORE1.DangerDiscardZone));
//            Assert.That(cardAdversity.Wasted.IsActive, Is.False);
//        }

//        [UnityTest]
//        public IEnumerator OtherInvestigatorAttackCostExtraTurn()
//        {
//            Investigator investigator = _investigatorsProvider.First;
//            Investigator investigator2 = _investigatorsProvider.Second;
//            Card01164 cardAdversity = _cardsProvider.GetCard<Card01164>();
//            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1);
//            yield return PlaceOnlyScene();
//            yield return PlayThisInvestigator(investigator);
//            yield return PlayThisInvestigator(investigator2);
//            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, SceneCORE1.Hallway).Execute().AsCoroutine();
//            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator2, SceneCORE1.Hallway).Execute().AsCoroutine();

//            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.GhoulSecuaz, investigator.DangerZone).Execute().AsCoroutine();
//            yield return _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, cardAdversity).Execute().AsCoroutine();

//            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator2).Execute();
//            yield return ClickedClone(SceneCORE1.GhoulSecuaz, 0);
//            yield return ClickedMainButton();
//            yield return ClickedResourceButton();
//            yield return ClickedResourceButton();
//            yield return ClickedMainButton();
//            yield return taskGameAction.AsCoroutine();

//            Assert.That(cardAdversity.CurrentZone, Is.EqualTo(investigator.DangerZone));
//            Assert.That(cardAdversity.Wasted.IsActive, Is.False);
//        }

//        [UnityTest]
//        public IEnumerator AttackWithWeaponCostExtraTurn()
//        {
//            Investigator investigator = _investigatorsProvider.First;
//            Card01164 cardAdversity = _cardsProvider.GetCard<Card01164>();
//            Card01506 weapon = _cardsProvider.GetCard<Card01506>();
//            _ = MustBeRevealedThisToken(ChallengeTokenType.Fail).ContinueWith((_) => MustBeRevealedThisToken(ChallengeTokenType.Value1));
//            yield return PlaceOnlyScene();
//            yield return PlayThisInvestigator(investigator);
//            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, SceneCORE1.Hallway).Execute().AsCoroutine();
//            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.GhoulSecuaz, investigator.DangerZone).Execute().AsCoroutine();
//            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(weapon, investigator.AidZone).Execute().AsCoroutine();
//            yield return _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, cardAdversity).Execute().AsCoroutine();

//            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
//            yield return ClickedIn(weapon);
//            yield return ClickedIn(SceneCORE1.GhoulSecuaz);
//            yield return ClickedMainButton();
//            yield return ClickedResourceButton();
//            yield return AssertThatIsNotClickable(SceneCORE1.Attic);
//            yield return ClickedMainButton();
//            yield return ClickedMainButton();
//            yield return taskGameAction.AsCoroutine();

//            Assert.That(cardAdversity.CurrentZone, Is.EqualTo(SceneCORE1.DangerDiscardZone));
//            Assert.That(cardAdversity.Wasted.IsActive, Is.False);
//        }

//        [UnityTest]
//        public IEnumerator TakeResources()
//        {
//            Investigator investigator = _investigatorsProvider.First;
//            Card01164 cardAdversity = _cardsProvider.GetCard<Card01164>();
//            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_1);
//            yield return PlaceOnlyScene();
//            yield return PlayThisInvestigator(investigator);
//            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, SceneCORE1.Hallway).Execute().AsCoroutine();
//            yield return _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, cardAdversity).Execute().AsCoroutine();
//            Assert.That(cardAdversity.Wasted.IsActive, Is.False);

//            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
//            yield return ClickedResourceButton();
//            yield return ClickedResourceButton();
//            yield return ClickedResourceButton();
//            yield return ClickedMainButton();
//            yield return ClickedMainButton();
//            yield return taskGameAction.AsCoroutine();

//            Assert.That(cardAdversity.CurrentZone, Is.EqualTo(investigator.DangerZone));
//            Assert.That(cardAdversity.Wasted.IsActive, Is.False);
//        }

//        [UnityTest]
//        public IEnumerator TakeResources2()
//        {
//            Investigator investigator = _investigatorsProvider.First;
//            Card01164 cardAdversity = _cardsProvider.GetCard<Card01164>();
//            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_1);
//            yield return PlaceOnlyScene();
//            yield return PlayThisInvestigator(investigator);
//            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, SceneCORE1.Hallway).Execute().AsCoroutine();
//            yield return _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, cardAdversity).Execute().AsCoroutine();
//            Assert.That(cardAdversity.Wasted.IsActive, Is.False);

//            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
//            yield return ClickedResourceButton();
//            yield return ClickedIn(SceneCORE1.Attic);
//            yield return AssertThatIsNotClickable(SceneCORE1.Attic);
//            yield return ClickedMainButton();
//            yield return ClickedMainButton();
//            yield return taskGameAction.AsCoroutine();

//            Assert.That(cardAdversity.CurrentZone, Is.EqualTo(investigator.DangerZone));
//            Assert.That(cardAdversity.Wasted.IsActive, Is.False);
//        }
//    }
//}