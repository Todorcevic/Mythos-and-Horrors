
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{

    public class CardAdversity01515Tests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator TakeShock()
        {
            Investigator investigator = _investigatorsProvider.Fourth;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01515 adversityCard = _cardsProvider.GetCard<Card01515>();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(investigator.DeckZone.Cards.Take(5), investigator.DiscardZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(adversityCard, investigator.DeckZone, isFaceDown: true)).AsCoroutine();
            Assert.That(investigator.DiscardZone.Cards.Count(), Is.EqualTo(5));
            yield return _gameActionsProvider.Create(new DrawAidGameAction(investigator)).AsCoroutine();

            Assert.That(investigator.DiscardZone.Cards.Count(), Is.EqualTo(1));
            Assert.That(investigator.FearRecived.Value, Is.EqualTo(2));
        }
    }
}
