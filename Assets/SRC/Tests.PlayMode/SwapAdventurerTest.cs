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
        private readonly bool DEBUG_MODE = false;
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

            yield return _sut.Select(adventurer2).WaitForCompletion();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_sut.GetPrivateMember<Transform>("_leftPosition").GetComponentInChildren<CardView>().Card,
                Is.EqualTo(adventurer1.AdventurerCard));
            Assert.That(_sut.GetPrivateMember<Transform>("_playPosition").GetComponentInChildren<CardView>().Card,
                Is.EqualTo(adventurer2.AdventurerCard));
        }
    }
}
