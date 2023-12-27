using MythsAndHorrors.GameRules;
using MythsAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythsAndHorrors.PlayMode.Tests
{
    public class InitalDrawGameActionTests : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] private readonly AdventurersProvider _adventurersProvider;
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly CardsProvider _cardsProvider;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator InitialDrawGameAction()
        {
            //DEBUG_MODE = true;
            _prepareGameUseCase.Execute();
            Adventurer adventurer = _adventurersProvider.AllAdventurers[0];
            Card card = _cardsProvider.GetCard("01517");
            yield return _gameActionFactory.Create<MoveCardsGameAction>().Run(card, adventurer.DeckZone).AsCoroutine();
            yield return _gameActionFactory.Create<InitialDrawGameAction>().Run(adventurer).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(adventurer.HandZone.Cards.Contains(card));
            yield return null;
        }

        [UnityTest]
        public IEnumerator InitialDrawWeaknessGameAction()
        {
            //DEBUG_MODE = true;
            _prepareGameUseCase.Execute();
            Adventurer adventurer = _adventurersProvider.AllAdventurers[0];
            Card weaknessCard = _cardsProvider.GetCard("01507");
            Card normalCard = _cardsProvider.GetCard("01517");
            yield return _gameActionFactory.Create<MoveCardsGameAction>().Run(new System.Collections.Generic.List<Card>() { normalCard, weaknessCard }, adventurer.DeckZone).AsCoroutine();
            yield return _gameActionFactory.Create<InitialDrawGameAction>().Run(adventurer).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(adventurer.HandZone.Cards.Contains(normalCard));
            Assert.That(adventurer.DiscardZone.Cards.Contains(weaknessCard));
            yield return null;
        }
    }
}
