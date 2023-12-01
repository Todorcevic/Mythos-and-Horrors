using DG.Tweening;
using MythsAndHorrors.GameRules;
using MythsAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythsAndHorrors.PlayMode.Tests
{
    [TestFixture]
    public class SwapAdventurerTest : TestBase
    {
        private readonly bool DEBUG_MODE = true;
        [Inject] private readonly SwapAdventurerComponent _sut;
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] private readonly AdventurersProvider _adventurersProvider;
        [Inject] private readonly CardMoverPresenter _cardMoverPresenter;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Swap()
        {
            _prepareGameUseCase.Execute();
            Adventurer adventurer1 = _adventurersProvider.AllAdventurers[0];
            Adventurer adventurer2 = _adventurersProvider.AllAdventurers[1];
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

            while (true)
            {
                yield return PressAnyKey();
                yield return _sut.Select(adventurer2).WaitForCompletion();
                yield return PressAnyKey();
                yield return _sut.Select(adventurer1).WaitForCompletion();
            }
            yield return PressAnyKey();
            yield return _sut.Select(adventurer2).WaitForCompletion();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_sut.GetPrivateMember<Transform>("_leftPosition").GetComponentInChildren<CardView>(true).Card,
                Is.EqualTo(adventurer1.AdventurerCard));
            Assert.That(_sut.GetPrivateMember<Transform>("_playPosition").GetComponentInChildren<CardView>().Card,
                Is.EqualTo(adventurer2.AdventurerCard));
        }
    }
}
