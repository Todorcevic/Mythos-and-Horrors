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
            yield return BuilCard("01551", investigator);
            Card01551 conditionCard = _cardsProvider.GetCard<Card01551>();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(conditionCard, investigator.HandZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new SpawnCreatureGameAction(SceneCORE1.GhoulGelid, investigator.CurrentPlace)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
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
            yield return BuilCard("01551", investigator);
            Card01551 conditionCard = _cardsProvider.GetCard<Card01551>();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(conditionCard, investigator.HandZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, SceneCORE1.Hallway)).AsCoroutine();
            yield return _gameActionsProvider.Create(new SpawnCreatureGameAction(SceneCORE1.GhoulGelid, SceneCORE1.Attic)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedTokenButton();
            yield return ClickedTokenButton();
            yield return ClickedIn(SceneCORE1.Attic);
            //Assert.That(conditionCard.CanBePlayed, Is.False);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(SceneCORE1.GhoulGelid.CurrentZone, Is.EqualTo(investigator.DangerZone));
        }
    }
}
