using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardAdversity01165Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator CantPlaySupplyAndCondition()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01165 cardAdversity = _cardsProvider.GetCard<Card01165>();
            Card01506 supply = _cardsProvider.GetCard<Card01506>();
            yield return StartingScene(withResources: true);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(supply, investigator.HandZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new DrawGameAction(investigator, cardAdversity)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new RoundGameAction());
            yield return ClickedIn(investigator.AvatarCard);
            Assert.That(supply.CanBePlayed, Is.False);
            yield return WasteAllTurns();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(cardAdversity.CurrentZone, Is.EqualTo(SceneCORE1.DangerDiscardZone));
        }


        [UnityTest]
        public IEnumerator CantPlaySupplyAndConditionWithOptianalReaction()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01165 cardAdversity = _cardsProvider.GetCard<Card01165>();
            Card01510 condition = _cardsProvider.GetCard<Card01510>();
            yield return StartingScene(withResources: true);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(condition, investigator.HandZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new DrawGameAction(investigator, cardAdversity)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new RoundGameAction());
            yield return ClickedIn(investigator.AvatarCard);
            yield return WasteAllTurns();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(cardAdversity.CurrentZone, Is.EqualTo(SceneCORE1.DangerDiscardZone));
        }
    }
}