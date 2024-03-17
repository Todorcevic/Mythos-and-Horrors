using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{

    public class InitalDrawGameActionTests : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator InitialDrawGameAction()
        {
            _prepareGameUseCase.Execute();
            Investigator investigator = _investigatorsProvider.First;
            Card card = _cardsProvider.GetCard("01517");
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(card, investigator.DeckZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new InitialDrawGameAction(investigator)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(investigator.HandZone.Cards.Contains(card));
            yield return null;
        }

        [UnityTest]
        public IEnumerator InitialDrawWeaknessGameAction()
        {
            _prepareGameUseCase.Execute();
            Investigator investigator = _investigatorsProvider.First;
            Card weaknessCard = _cardsProvider.GetCard("01507");
            Card normalCard = _cardsProvider.GetCard("01517");
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(new[] { normalCard, weaknessCard }, investigator.DeckZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new InitialDrawGameAction(investigator)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(investigator.HandZone.Cards.Contains(normalCard));
            Assert.That(investigator.DiscardZone.Cards.Contains(weaknessCard));
            yield return null;
        }
    }
}
