using MythsAndHorrors.GameRules;
using MythsAndHorrors.GameView;
using NUnit.Framework;
using Zenject;

namespace MythsAndHorrors.EditMode.Tests
{
    [TestFixture]
    public class AdveturerLoaderTests : SetupAutoInject
    {
        private const string JSON_TEST_DATA_PATH = "Assets/Data/Tests/Adventurers/";

        [Inject] private readonly AdventurerLoaderUseCase _sut;
        [Inject] private readonly AdventurersProvider _adventurersProvider;

        /*******************************************************************/
        [Test]
        public void LoadManyAdventurer()
        {
            _sut.Execute(JSON_TEST_DATA_PATH + "01501.json");
            _sut.Execute(JSON_TEST_DATA_PATH + "01502.json");
            _sut.Execute(JSON_TEST_DATA_PATH + "01503.json");

            Assert.That(_adventurersProvider.Leader.AdventurerCard.Info.Code, Is.EqualTo("01501"));
            Assert.That(_adventurersProvider.AllAdventurers.Count, Is.EqualTo(3));
        }
    }
}
