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
        [Inject] private readonly AdventurersProvider _adventurersProvider;
        [Inject] private readonly ZoneViewsManager _zonesManager;
        [Inject] private readonly CardViewGeneratorComponent _cardGenerator;
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Swap()
        {
            _prepareGameUseCase.Execute();
            Adventurer adventurer1 = _adventurersProvider.AllAdventurers[0];
            Adventurer adventurer2 = _adventurersProvider.AllAdventurers[1];
            CardView oneCard = _cardGenerator.BuildCard(adventurer1.AdventurerCard);
            yield return _zonesManager.Get(adventurer1.HandZone).EnterCard(oneCard).WaitForCompletion();
            CardView twoCard = _cardGenerator.BuildCard(adventurer2.AdventurerCard);
            yield return _zonesManager.Get(adventurer2.HandZone).EnterCard(twoCard).WaitForCompletion();

            yield return _sut.Select(adventurer2).WaitForCompletion();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_sut.GetPrivateMember<Transform>("_leftPosition").GetComponentInChildren<CardView>(), Is.EqualTo(oneCard));
            Assert.That(_sut.GetPrivateMember<Transform>("_playPosition").GetComponentInChildren<CardView>(), Is.EqualTo(twoCard));
        }


    }
}
