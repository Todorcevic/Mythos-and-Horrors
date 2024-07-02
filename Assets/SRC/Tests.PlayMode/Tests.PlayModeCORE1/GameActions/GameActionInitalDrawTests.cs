using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class GameActionInitalDrawTests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator InitialDrawBasic()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<DiscardGameAction>().SetWith(investigator.HandZone.Cards.First()).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<InitialDrawGameAction>().SetWith(investigator).Start().AsCoroutine();

            Assert.That(investigator.HandZone.Cards.Count(), Is.EqualTo(5));
        }

        [UnityTest]
        public IEnumerator InitialDrawWithWeakness()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return PlayThisInvestigator(investigator);
            Card weaknessCard = _cardsProvider.GetCard<Card01507>();
            Card normalCard = _cardsProvider.GetCard<Card01517>();
            yield return _gameActionsProvider.Create<DiscardGameAction>().SetWith(investigator.HandZone.Cards.First()).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(new[] { normalCard, weaknessCard }, investigator.DeckZone).Start().AsCoroutine();

            yield return _gameActionsProvider.Create<InitialDrawGameAction>().SetWith(investigator).Start().AsCoroutine();

            Assert.That(investigator.HandZone.Cards.Contains(normalCard));
            Assert.That(investigator.DiscardZone.Cards.Contains(weaknessCard));
        }
    }
}
