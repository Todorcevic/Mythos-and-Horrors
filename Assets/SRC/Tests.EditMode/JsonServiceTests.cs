using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using MythsAndHorrors.GameView;
using MythsAndHorrors.GameRules;
using Zenject;

namespace MythsAndHorrors.EditMode.Tests
{
    [TestFixture]
    public class JsonServiceTests : OneTimeAutoInject
    {
        private const string JSON_TEST_DATA_PATH = "Assets/Data/Tests/Test.json";
        [Inject] private readonly JsonService _sut;
        [Inject] private readonly FilesPath _filesPath;
        [Inject] private readonly DataSaveLoaderUseCase _dataSaveLoaderUseCase;

        /*******************************************************************/
        [SetUp]
        public void SetUp()
        {
            _dataSaveLoaderUseCase.Execute();
        }

        [Test]
        public void CardInfoJson_File_Exist()
        {
            Assert.That(File.Exists(_filesPath.JSON_CARDINFO_PATH), Is.True);
        }

        [Test]
        public void DeserializeCardsUseCase_With_Safe_Data()
        {
            List<CardInfo> result = _sut.CreateDataFromFile<List<CardInfo>>(JSON_TEST_DATA_PATH);

            Assert.That(result[0].Name, Is.EqualTo("First Adventurer"));
            Assert.That(result[1].CardType, Is.EqualTo(CardType.Investigator));
        }

        [Test]
        public void DeserializeCardsUseCase_With_Real_Data()
        {
            List<CardInfo> result = _sut.CreateDataFromFile<List<CardInfo>>(_filesPath.JSON_CARDINFO_PATH);

            Assert.That(result.Count, Is.GreaterThan(0));
        }
    }
}
