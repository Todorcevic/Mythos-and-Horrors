using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class GameActionInitalDrawTests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator InitialDrawBasic()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new DiscardGameAction(investigator.HandZone.Cards.First())).AsCoroutine();
            yield return _gameActionsProvider.Create(new InitialDrawGameAction(investigator)).AsCoroutine();

            Assert.That(investigator.HandZone.Cards.Count(), Is.EqualTo(5));
        }

        [UnityTest]
        public IEnumerator InitialDrawWithWeakness()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return PlayThisInvestigator(investigator);
            Card weaknessCard = _cardsProvider.GetCard<Card01507>();
            Card normalCard = _cardsProvider.GetCard<Card01517>();
            yield return _gameActionsProvider.Create(new DiscardGameAction(investigator.HandZone.Cards.First())).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(new[] { normalCard, weaknessCard }, investigator.DeckZone)).AsCoroutine();

            yield return _gameActionsProvider.Create(new InitialDrawGameAction(investigator)).AsCoroutine();

            Assert.That(investigator.HandZone.Cards.Contains(normalCard));
            Assert.That(investigator.DiscardZone.Cards.Contains(weaknessCard));
        }
    }
}
