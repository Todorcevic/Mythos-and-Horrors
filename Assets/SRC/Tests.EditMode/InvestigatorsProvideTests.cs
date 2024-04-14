using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.EditMode.Tests
{
    [TestFixture]
    public class InvestigatorsProvideTests : SetupAutoInject
    {
        [Inject] private readonly InvestigatorsProvider _sut;
        [Inject] private readonly DiContainer _diContainer;

        /*******************************************************************/
        [Test]
        public void AddManyInvestigators()
        {
            Investigator _doc1 = _diContainer.Instantiate<Investigator>();
            Investigator _doc2 = _diContainer.Instantiate<Investigator>();
            Investigator _doc3 = _diContainer.Instantiate<Investigator>();

            _sut.AddInvestigator(_doc1);
            _sut.AddInvestigator(_doc2);
            _sut.AddInvestigator(_doc3);

            Assert.That(_sut.AllInvestigators.Count(), Is.EqualTo(3));
            Assert.That(_sut.First, Is.EqualTo(_doc1));
            Assert.That(_doc1.Position, Is.EqualTo(1));
            Assert.That(_doc2.Position, Is.EqualTo(2));
            Assert.That(_doc3.Position, Is.EqualTo(3));
        }
    }
}
