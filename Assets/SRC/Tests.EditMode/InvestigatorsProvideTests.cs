using MythsAndHorrors.GameRules;
using NUnit.Framework;
using Zenject;

namespace MythsAndHorrors.EditMode.Tests
{
    [TestFixture]
    public class InvestigatorsProvideTests : SetupAutoInject
    {
        [Inject] private readonly InvestigatorsProvider _sut;

        /*******************************************************************/
        [Test]
        public void AddManyInvestigators()
        {
            Investigator _doc1 = new();
            Investigator _doc2 = new();
            Investigator _doc3 = new();
            _sut.AddInvestigator(_doc1);
            _sut.AddInvestigator(_doc2);
            _sut.AddInvestigator(_doc3);

            Assert.That(_sut.AllInvestigators.Count, Is.EqualTo(3));
            Assert.That(_sut.Leader, Is.EqualTo(_doc1));
            Assert.That(_sut.GetInvestigatorPosition(_doc1), Is.EqualTo(1));
            Assert.That(_sut.GetInvestigatorPosition(_doc2), Is.EqualTo(2));
            Assert.That(_sut.GetInvestigatorPosition(_doc3), Is.EqualTo(3));
        }
    }
}
