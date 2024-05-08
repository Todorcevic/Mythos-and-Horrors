using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class InitalDrawGameActionTests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator InitialDrawGameAction()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return _preparationScene.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new DiscardGameAction(investigator.HandZone.Cards.First())).AsCoroutine();
            yield return _gameActionsProvider.Create(new InitialDrawGameAction(investigator)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(investigator.HandZone.Cards.Count(), Is.EqualTo(5));
            yield return null;
        }

        [UnityTest]
        public IEnumerator InitialDrawWeaknessGameAction()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return _preparationScene.PlayThisInvestigator(investigator);
            Card weaknessCard = _cardsProvider.GetCard<Card01507>();
            Card normalCard = _cardsProvider.GetCard<Card01517>();
            yield return _gameActionsProvider.Create(new DiscardGameAction(investigator.HandZone.Cards.First())).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(new[] { normalCard, weaknessCard }, investigator.DeckZone)).AsCoroutine();

            yield return _gameActionsProvider.Create(new InitialDrawGameAction(investigator)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(investigator.HandZone.Cards.Contains(normalCard));
            Assert.That(investigator.DiscardZone.Cards.Contains(weaknessCard));
            yield return null;
        }
    }
}
