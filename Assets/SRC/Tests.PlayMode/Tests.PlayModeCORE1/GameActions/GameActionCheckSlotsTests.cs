using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class GameActionCheckSlotsTests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator CheckSlots()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card toPlay = _cardsProvider.GetCard<Card01518>();
            Card toPlay2 = _cardsProvider.GetCard<Card01521>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(toPlay, investigator.AidZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(toPlay2, investigator.HandZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(toPlay2);
            yield return ClickedIn(toPlay);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.AidZone.Cards.Contains(toPlay2), Is.True);
            Assert.That(investigator.DiscardZone.Cards.Contains(toPlay), Is.True);
        }

        [UnityTest]
        public IEnumerator CheckManySlots()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card toPlay = _cardsProvider.GetCard<Card01518>();
            Card toPlay2 = _cardsProvider.GetCard<Card01521>();
            Card toPlay3 = _cardsProvider.GetCard<Card01530>();
            Card toPlay4 = _cardsProvider.GetCard<Card01516>();
            Card toPlay5 = _cardsProvider.GetCard<Card01506>();

            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(toPlay, investigator.AidZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(toPlay3, investigator.AidZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(toPlay4, investigator.AidZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(new[] { toPlay2, toPlay5 }, investigator.AidZone).Execute();
            yield return ClickedIn(toPlay3);
            yield return ClickedIn(toPlay);
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.AidZone.Cards.Contains(toPlay2), Is.True);
            Assert.That(investigator.DiscardZone.Cards.Contains(toPlay), Is.True);
            Assert.That(investigator.DiscardZone.Cards.Contains(toPlay3), Is.True);
        }
    }
}
