using DG.Tweening;
using MythsAndHorrors.GameRules;
using MythsAndHorrors.GameView;
using MythsAndHorrors.GameView.Tests.Assets.SRC.GameView.Tests.Utils;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine.TestTools;
using Zenject;

namespace MythsAndHorrors.Gameview.Tests
{
    [TestFixture]
    public class ZonesBehaviourTests : TestBase
    {
        [Inject] private readonly ZonesManager _zonesManager;
        private CardBuilder _cardBuilder;

        [UnitySetUp]
        public override IEnumerator SetUp()
        {
            yield return base.SetUp();
            _cardBuilder = SceneContainer.Instantiate<CardBuilder>();
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Basic()
        {
            ZoneView sut = _zonesManager.Get("AdventurerZone");
            CardView _doc = _cardBuilder.BuildOne();

            yield return sut.MoveCard(_doc).WaitForCompletion();

            Assert.That(_doc.transform.parent, Is.EqualTo(sut.transform));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Row()
        {
            ZoneView sut = _zonesManager.Get("AidZone");
            CardView[] _doc = _cardBuilder.BuildManySame(3);

            foreach (CardView card in _doc)
            {
                yield return sut.MoveCard(card).WaitForCompletion();
            }

            yield return PressAnyKey();

            Assert.That(_doc.First().transform.parent, Is.EqualTo(sut.transform));
            yield return null;
        }
    }
}
