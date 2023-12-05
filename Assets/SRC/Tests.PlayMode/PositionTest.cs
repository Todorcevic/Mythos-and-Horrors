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
    public class PositionTest : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] private readonly AdventurersProvider _adventurersProvider;
        [Inject] private readonly CardMoverPresenter _cardMoverPresenter;
        [Inject] private readonly GameStateService _gameStateService;
        [Inject] private readonly ZonesProvider _zonesProvider;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator All_Zones_With_Cards()
        {
            DEBUG_MODE = true;
            ViewValues.FAST_TIME_ANIMATION = 0f;
            _prepareGameUseCase.Execute();
            Adventurer adventurer1 = _adventurersProvider.AllAdventurers[0];

            yield return _cardMoverPresenter.MoveCardToZoneAsync(adventurer1.AdventurerCard, adventurer1.AdventurerZone).AsCoroutine();
            yield return _cardMoverPresenter.MoveCardToZoneAsync(adventurer1.Cards[1], adventurer1.DiscardZone).AsCoroutine();
            yield return _cardMoverPresenter.MoveCardToZoneAsync(adventurer1.Cards[2], adventurer1.DeckZone).AsCoroutine();
            for (int i = 0; i < 5; i++)
            {
                yield return _cardMoverPresenter.MoveCardToZoneAsync(adventurer1.Cards[i + 3], adventurer1.AidZone).AsCoroutine();
            }
            for (int i = 0; i < 5; i++)
            {
                yield return _cardMoverPresenter.MoveCardToZoneAsync(adventurer1.Cards[i + 9], adventurer1.DangerZone).AsCoroutine();
            }
            for (int i = 0; i < 8; i++)
            {
                yield return _cardMoverPresenter.MoveCardToZoneAsync(adventurer1.Cards[i + 15], adventurer1.HandZone).AsCoroutine();
            }

            yield return _cardMoverPresenter.MoveCardToZoneAsync(_gameStateService.CurrentScene.Cards[0], _zonesProvider.PlotZone).AsCoroutine();
            yield return _cardMoverPresenter.MoveCardToZoneAsync(_gameStateService.CurrentScene.Cards[1], _zonesProvider.GoalZone).AsCoroutine();
            yield return _cardMoverPresenter.MoveCardToZoneAsync(_gameStateService.CurrentScene.Cards[3], _zonesProvider.DangerDeckZone).AsCoroutine();
            yield return _cardMoverPresenter.MoveCardToZoneAsync(_gameStateService.CurrentScene.Cards[4], _zonesProvider.DangerDiscardZone).AsCoroutine();

            int k = 0;
            for (int i = 0; i < _zonesProvider.PlaceZone.GetLength(0); i++)
            {
                for (int j = 0; j < _zonesProvider.PlaceZone.GetLength(1); j++)
                {
                    yield return _cardMoverPresenter.MoveCardToZoneAsync(_gameStateService.CurrentScene.Cards[5 + k++], _zonesProvider.PlaceZone[i, j]).AsCoroutine();
                }
            }

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(true);
        }
    }
}
