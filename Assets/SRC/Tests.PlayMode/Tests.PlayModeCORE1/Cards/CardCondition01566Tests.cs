using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using System.Threading.Tasks;
using UnityEngine;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardCondition01566Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator Evade()
        {
            Investigator investigator = _investigatorsProvider.Fourth;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_1);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01566 conditionCard = _cardsProvider.GetCard<Card01566>();

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(conditionCard, investigator.HandZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(SceneCORE1.GhoulGelid, investigator.CurrentPlace).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(conditionCard);
            yield return ClickedIn(SceneCORE1.GhoulGelid);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.CurrentTurns.ValueBeforeUpdate, Is.EqualTo(2));
            Assert.That(SceneCORE1.GhoulGelid.Exausted.IsActive, Is.True);
        }

        [UnityTest]
        public IEnumerator Dazzle()
        {
            Investigator investigator = _investigatorsProvider.Fourth;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Fail);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01566 conditionCard = _cardsProvider.GetCard<Card01566>();

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(conditionCard, investigator.HandZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(SceneCORE1.GhoulGelid, investigator.CurrentPlace).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(conditionCard);
            yield return ClickedIn(SceneCORE1.GhoulGelid);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.CurrentTurns.ValueBeforeUpdate, Is.EqualTo(1));
            Assert.That(SceneCORE1.GhoulGelid.Exausted.IsActive, Is.False);
        }
    }
}
