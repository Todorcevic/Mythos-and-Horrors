﻿using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.EditMode.Tests
{
    [TestFixture]
    public class SceneLoaderTests : SetupAutoInject
    {
        [Inject] private readonly SceneLoaderUseCase _sut;
        [Inject] private readonly DataSaveUseCase _dataSaveLoaderUseCase;

        /*******************************************************************/
        [Test]
        public void LoadScene()
        {
            _dataSaveLoaderUseCase.Load();

            _sut.Execute();

            Assert.That(_chaptersProvider.CurrentScene.Info.Name, Is.EqualTo("El encuentro"));
            Assert.That(_chaptersProvider.CurrentScene.Info.Cards.First().Info.Code, Is.EqualTo("01108"));
        }
    }
}