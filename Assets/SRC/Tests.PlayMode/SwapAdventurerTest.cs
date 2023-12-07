using DG.Tweening;
using MythsAndHorrors.GameRules;
using MythsAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;
using Zenject;

namespace MythsAndHorrors.PlayMode.Tests
{
    [TestFixture]
    public class SwapAdventurerTest : TestBase
    {
        [Inject] private readonly SwapAdventurerComponent _sut;
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] private readonly AdventurersProvider _adventurersProvider;
        [Inject] private readonly CardMoverPresenter _cardMoverPresenter;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Swap()
        {
            //DEBUG_MODE = true;
            _prepareGameUseCase.Execute();
            Adventurer adventurer1 = _adventurersProvider.AllAdventurers[0];
            Adventurer adventurer2 = _adventurersProvider.AllAdventurers[1];

            if (DEBUG_MODE) ViewValues.FAST_TIME_ANIMATION = 0;
            yield return _cardMoverPresenter.MoveCardToZoneAsync(adventurer1.AdventurerCard, adventurer1.HandZone).AsCoroutine();
            yield return _cardMoverPresenter.MoveCardToZoneAsync(adventurer2.AdventurerCard, adventurer2.HandZone).AsCoroutine();
            yield return _cardMoverPresenter.MoveCardToZoneAsync(adventurer1.Cards[2], adventurer1.AidZone).AsCoroutine();
            yield return _cardMoverPresenter.MoveCardToZoneAsync(adventurer2.Cards[2], adventurer2.AidZone).AsCoroutine();

            yield return _cardMoverPresenter.MoveCardToZoneAsync(adventurer1.Cards[3], adventurer1.DiscardZone).AsCoroutine();
            yield return _cardMoverPresenter.MoveCardToZoneAsync(adventurer2.Cards[3], adventurer2.DiscardZone).AsCoroutine();

            yield return _cardMoverPresenter.MoveCardToZoneAsync(adventurer1.Cards[4], adventurer1.DeckZone).AsCoroutine();
            yield return _cardMoverPresenter.MoveCardToZoneAsync(adventurer2.Cards[4], adventurer2.DeckZone).AsCoroutine();

            yield return _cardMoverPresenter.MoveCardToZoneAsync(adventurer1.Cards[5], adventurer1.AdventurerZone).AsCoroutine();
            yield return _cardMoverPresenter.MoveCardToZoneAsync(adventurer2.Cards[5], adventurer2.AdventurerZone).AsCoroutine();

            yield return _cardMoverPresenter.MoveCardToZoneAsync(adventurer1.Cards[6], adventurer1.DangerZone).AsCoroutine();
            yield return _cardMoverPresenter.MoveCardToZoneAsync(adventurer2.Cards[6], adventurer2.DangerZone).AsCoroutine();

            if (DEBUG_MODE) ViewValues.FAST_TIME_ANIMATION = 0.25f;
            while (DEBUG_MODE)
            {
                yield return PressAnyKey();
                yield return _sut.Select(adventurer2).WaitForCompletion();
                yield return PressAnyKey();
                yield return _sut.Select(adventurer1).WaitForCompletion();
            }

            yield return _sut.Select(adventurer2).WaitForCompletion();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(_sut.AdventurerSelected, Is.EqualTo(adventurer2));
        }
    }
}
