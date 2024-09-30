using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using UnityEngine;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardCreature01121bTests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator HunterBuff()
        {
            Investigator investigator = _investigatorsProvider.Second;
            CardCreature maskedHunter = _cardsProvider.GetCard<Card01121>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1).ContinueWith((_) => MustBeRevealedThisToken(ChallengeTokenType.Value1)); ;
            yield return StartingScene();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.First, SceneCORE2.North).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, SceneCORE2.East).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.Third, SceneCORE2.University).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<GainKeyGameAction>().SetWith(investigator, investigator.CurrentPlace.Keys, 2).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<DrawGameAction>().SetWith(_investigatorsProvider.First, maskedHunter).Execute().AsCoroutine();

            AssumeThat(maskedHunter.CurrentPlace == investigator.CurrentPlace);

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();

            AssumeThat(investigator.Keys.Value == 2);

            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();
            yield return _gameActionsProvider.Create<DefeatCardGameAction>().SetWith(maskedHunter, investigator.InvestigatorCard).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<ResetAllInvestigatorsTurnsGameAction>().Execute().AsCoroutine();

            gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.Keys.Value, Is.EqualTo(3));
        }

        [UnityTest]
        public IEnumerator HunterBuffCantPay()
        {
            Investigator investigator = _investigatorsProvider.Second;
            CardCreature maskedHunter = _cardsProvider.GetCard<Card01121>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return PlayThisInvestigator(_investigatorsProvider.Third);
            yield return _gameActionsProvider.Create<GainKeyGameAction>().SetWith(investigator, investigator.CurrentPlace.Keys, 4).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<GainKeyGameAction>().SetWith(investigator, SceneCORE2.East.Keys, 4).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(maskedHunter, investigator.DangerZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return AssertThatIsNotClickable(SceneCORE2.CurrentGoal);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.Keys.Value, Is.EqualTo(8));
        }

        [UnityTest]
        public IEnumerator HunterBuffCantPayShadow()
        {
            Investigator investigator = _investigatorsProvider.Second;
            CardCreature maskedHunter = _cardsProvider.GetCard<Card01121>();
            Card01135 shadow = _cardsProvider.GetCard<Card01135>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<GainKeyGameAction>().SetWith(investigator, investigator.CurrentPlace.Keys, 4).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(maskedHunter, investigator.DangerZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, shadow).Execute();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.DamageRecived.Value, Is.EqualTo(2));
        }
    }
}
