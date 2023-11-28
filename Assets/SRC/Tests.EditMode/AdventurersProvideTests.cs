using MythsAndHorrors.GameRules;
using NUnit.Framework;
using Zenject;

namespace MythsAndHorrors.EditMode.Tests
{
    [TestFixture]
    public class AdventurersProvideTests : SetupAutoInject
    {
        [Inject] private readonly AdventurersProvider _sut;

        /*******************************************************************/
        [Test]
        public void AddManyAdventurer()
        {
            Adventurer _doc1 = new();
            Adventurer _doc2 = new();
            Adventurer _doc3 = new();
            _sut.AddAdventurer(_doc1);
            _sut.AddAdventurer(_doc2);
            _sut.AddAdventurer(_doc3);

            Assert.That(_sut.AllAdventurers.Count, Is.EqualTo(3));
            Assert.That(_sut.Leader, Is.EqualTo(_doc1));
            Assert.That(_sut.GetAdventurerPosition(_doc1), Is.EqualTo(1));
            Assert.That(_sut.GetAdventurerPosition(_doc2), Is.EqualTo(2));
            Assert.That(_sut.GetAdventurerPosition(_doc3), Is.EqualTo(3));
        }
    }
}
