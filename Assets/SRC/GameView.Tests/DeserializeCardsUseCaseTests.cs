using GameRules;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;

namespace GameView.Tests
{
    //public class FakeSerializer : ISerializer
    //{
    //    public T CreateDataFromFile<T>(string pathAndNameJsonFile)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public T CreateDataFromResources<T>(string pathAndNameJsonFile)
    //    {
    //        throw new System.NotImplementedException();
    //    }
    //}

    [TestFixture]
    public class DeserializeCardsUseCaseTests
    {
        private readonly string path = Path.Combine(Application.persistentDataPath, "test.json");
        [Inject] private readonly DeserializeCardsUseCase sut;

        [SetUp]
        public void SetUp()
        {
            //StaticContext.Container.Unbind<ISerializer>();
            //StaticContext.Container.Bind<ISerializer>().To<FakeSerializer>().AsSingle();
            StaticContext.Container.Inject(this);

        }

        [Test]
        public void CardInfoJson_File_Exist()
        {
            Assert.IsTrue(File.Exists(FilesPath.JSON_DATA_PATH));
        }

        [Test]
        public void DeserializeCardsUseCase_With_Safe_String()
        {
            File.WriteAllText(path, "[\r\n  {\r\n    \"$type\": \"Tools.CardInfo, Tools\",\r\n    \"Description\": \"Hola Mondo\",\r\n    \"PackCode\": \"3123\",\r\n    \"Faction\": 3,\r\n    \"Cost\": 4,\r\n    \"CardType\": 0,\r\n    \"Code\": \"00001\",\r\n    \"Name\": \"First Investigator\"\r\n  },\r\n  {\r\n    \"$type\": \"Tools.CardInfo, Tools\",\r\n    \"Description\": \"AJJAJA\",\r\n    \"PackCode\": \"3\",\r\n    \"Faction\": 0,\r\n    \"Quantity\": 2,\r\n    \"CardType\": 4,\r\n    \"Code\": \"00002\",\r\n    \"Name\": \"Montro2\"\r\n  }\r\n]");

            List<Card> result = sut.Load(path);

            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void DeserializeCardsUseCase_With_Real_Data()
        {
            List<Card> result = sut.Load(FilesPath.JSON_DATA_PATH);

            Assert.That(result, Is.Not.Null);
        }

        [TearDown]
        public void CleanUp()
        {
            if (File.Exists(path)) File.Delete(path);
        }
    }
}
