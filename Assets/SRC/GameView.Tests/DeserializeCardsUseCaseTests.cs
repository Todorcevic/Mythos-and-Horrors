using GameRules;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;

namespace GameView.Tests
{
    [TestFixture]
    public class DeserializeCardsUseCaseTests : OneTimeAutoInject
    {
        private readonly string path = Path.Combine(Application.persistentDataPath, "test.json");
        [Inject] private readonly DeserializeCardsInfoUseCase sut;

        [Test]
        public void CardInfoJson_File_Exist()
        {
            Assert.That(File.Exists(FilesPath.JSON_DATA_PATH), Is.True);
        }

        [Test]
        public void DeserializeCardsUseCase_With_Safe_Data()
        {
            File.WriteAllText(path, "[\r\n  {\r\n    \"$type\": \"Tools.CardInfo, Tools\",\r\n    \"Description\": \"Hola Mondo\",\r\n    \"PackCode\": \"3123\",\r\n    \"Faction\": 3,\r\n    \"Cost\": 4,\r\n    \"CardType\": 0,\r\n    \"Code\": \"00001\",\r\n    \"Name\": \"First Adventurer\"\r\n  },\r\n  {\r\n    \"$type\": \"Tools.CardInfo, Tools\",\r\n    \"Description\": \"AJJAJA\",\r\n    \"PackCode\": \"3\",\r\n    \"Faction\": 0,\r\n    \"Quantity\": 2,\r\n    \"CardType\": 4,\r\n    \"Code\": \"00002\",\r\n    \"Name\": \"Montro2\"\r\n  }\r\n]");

            List<CardInfo> result = sut.CreateFrom(path);

            Assert.That(result[0].Name, Is.EqualTo("First Adventurer"));
            Assert.That(result[1].CardType, Is.EqualTo(CardType.Creature));
        }

        [Test]
        public void DeserializeCardsUseCase_With_Real_Data()
        {
            List<CardInfo> result = sut.CreateFrom(FilesPath.JSON_DATA_PATH);

            Assert.That(result.Count, Is.GreaterThan(0));
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(path)) File.Delete(path);
        }
    }
}
