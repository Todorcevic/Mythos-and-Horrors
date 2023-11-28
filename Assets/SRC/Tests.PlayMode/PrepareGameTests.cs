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
    public class PrepareGameTests : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _sut;
        [Inject] private readonly AdventurersProvider _adventurersProvider;

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
            yield return null;
        }
    }
}
