using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using MythosAndHorrors.GameView;
using MythosAndHorrors.GameRules;
using Zenject;

namespace MythosAndHorrors.EditMode.Tests
{
    [TestFixture]
    public class JsonServiceTests : OneTimeAutoInject
    {
        [Inject] private readonly JsonService _sut;
        [Inject] private readonly FilesPath _filesPath;
        [Inject] private readonly DataSaveUseCase _dataSaveLoaderUseCase;

        /*******************************************************************/
        [SetUp]
        public void SetUp()
        {
            _dataSaveLoaderUseCase.Load();
        }

        [Test]
        public void CardInfoJson_File_Exist()
        {
            Assert.That(File.Exists(_filesPath.JSON_CARDINFO_PATH), Is.True);
        }

        [Test]
        public void DeserializeCardsUseCase_With_Real_Data()
        {
            List<CardInfo> result = _sut.CreateDataFromFile<List<CardInfo>>(_filesPath.JSON_CARDINFO_PATH);

            Assert.That(result.Count, Is.GreaterThan(0));
        }
    }
}
