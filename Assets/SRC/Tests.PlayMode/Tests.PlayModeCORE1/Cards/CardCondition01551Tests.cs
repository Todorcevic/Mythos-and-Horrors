using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using System.Threading.Tasks;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardCondition01551Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator Attack()
        {
            Investigator investigator = _investigatorsProvider.Third;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return BuildCard("01551", investigator);
            Card01551 conditionCard = _cardsProvider.GetCard<Card01551>();

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(conditionCard, investigator.HandZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(SceneCORE1.GhoulGelid, investigator.CurrentPlace).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(conditionCard);
            yield return ClickedIn(SceneCORE1.GhoulGelid);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(SceneCORE1.GhoulGelid.DamageRecived.Value, Is.EqualTo(3));
            Assert.That(investigator.DamageRecived.Value, Is.EqualTo(0));
        }

        [UnityTest]
        public IEnumerator CantAttack()
        {
            Investigator investigator = _investigatorsProvider.Third;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return BuildCard("01551", investigator);
            Card01551 conditionCard = _cardsProvider.GetCard<Card01551>();

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(conditionCard, investigator.HandZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, SceneCORE1.Hallway).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(SceneCORE1.GhoulGelid, SceneCORE1.Attic).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedTokenButton();
            yield return ClickedTokenButton();
            yield return ClickedIn(SceneCORE1.Attic);
            if (TestsType == TestsType.Unit) Assert.That(conditionCard.CanBePlayed, Is.False);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(SceneCORE1.GhoulGelid.CurrentZone, Is.EqualTo(investigator.DangerZone));
        }
    }
}
