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
    public class PrepareGameUseCaseTests : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _sut;
        [Inject] private readonly AdventurersProvider _adventurersProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly GameStateService _gameStateService;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator PrepareGame()
        {
            //DEBUG_MODE = true;
            _sut.Execute();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_adventurersProvider.AllAdventurers.Count, Is.EqualTo(2));
            Assert.That(_cardsProvider.GetCard("01160").Info.Code, Is.EqualTo("01160"));
            Assert.That(_gameStateService.CurrentScene.Info.Name, Is.EqualTo("Scene1"));
            yield return null;
        }
    }
}
