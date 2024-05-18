using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class CardSupply01530Tests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator PlayFormHandFreeCost()
        {
            Card01530 supply = _cardsProvider.GetCard<Card01530>();
            Investigator investigator = _investigatorsProvider.Second;
            yield return PlayThisInvestigator(investigator, withAvatar: false);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(supply, investigator.HandZone)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(supply);
            Assert.That(investigator.CurrentTurns.Value, Is.EqualTo(3));
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(supply.CurrentZone, Is.EqualTo(investigator.AidZone));
        }

        [UnityTest]
        public IEnumerator BuffIntelligenceToOwnerTest()
        {
            Card cardWithBuff = _cardsProvider.GetCard<Card01530>();
            Investigator investigator = _investigatorsProvider.Second;
            yield return StartingScene();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardWithBuff, investigator.AidZone)).AsCoroutine();

            Assert.That(_investigatorsProvider.First.Intelligence.Value, Is.EqualTo(3));
            Assert.That(_investigatorsProvider.Second.Intelligence.Value, Is.EqualTo(6));

            yield return _gameActionsProvider.Create(new DiscardGameAction(cardWithBuff)).AsCoroutine();

            Assert.That(_investigatorsProvider.First.Intelligence.Value, Is.EqualTo(3));
            Assert.That(_investigatorsProvider.Second.Intelligence.Value, Is.EqualTo(5));
        }
    }
}
