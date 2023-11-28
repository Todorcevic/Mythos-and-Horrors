using MythsAndHorrors.GameRules;
using MythsAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
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
            SaveData saveData = new()
            {
                AdventurersSelected = new List<string>() { "01501", "01502" },
                SceneSelected = "COREScene1"
            };

            _sut.Execute(saveData);

            Assert.That(_adventurersProvider.AllAdventurers.Count, Is.EqualTo(2));
            Assert.That(_cardsProvider.GetCard("01160").Info.Code, Is.EqualTo("01160"));
            Assert.That(_gameStateService.CurrentScene.Name, Is.EqualTo("Scene1"));
            yield return null;
        }
    }
}
