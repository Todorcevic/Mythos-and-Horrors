using DG.Tweening;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythsAndHorrors.GameView.Tests
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

            yield return new WaitForSeconds(150);
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

            yield return new WaitForSeconds(150);
            Assert.That(_doc.First().transform.parent, Is.EqualTo(sut.transform));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Row_Creating_Holders()
        {
            ZoneView sut = _zonesManager.Get("AidZone");
            CardView[] _doc = _cardBuilder.BuildManySame(13);

            foreach (CardView card in _doc)
            {
                yield return sut.MoveCard(card).WaitForCompletion();
            }


            yield return new WaitForSeconds(150);
            Assert.That(_doc.First().transform.parent, Is.EqualTo(sut.transform));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Hand()
        {
            ZoneView sut = _zonesManager.Get("HandZone");
            ZoneView _doc2 = _zonesManager.Get("OutGameZone");
            CardView[] _doc = _cardBuilder.BuildManySame(2);

            foreach (CardView card in _doc)
            {
                yield return sut.MoveCard(card).WaitForCompletion();
            }

            yield return new WaitForSeconds(150);

            // yield return PressAnyKey();

            Assert.That(_doc.First().transform.parent, Is.EqualTo(sut.transform));
        }

        [UnityTest]
        public IEnumerator Remove_Card_In_Zone_Hand()
        {
            ZoneView sut = _zonesManager.Get("HandZone");
            ZoneView _doc2 = _zonesManager.Get("OutGameZone");
            CardView[] _doc = _cardBuilder.BuildManySame(5);

            foreach (CardView card in _doc)
            {
                yield return sut.MoveCard(card).WaitForCompletion();
            }

            yield return _doc2.MoveCard(_doc[0]);
            yield return sut.RemoveCard(_doc[0]);

            Assert.That(_doc.First().transform.parent, Is.EqualTo(sut.transform));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Deck()
        {
            ZoneView sut = _zonesManager.Get("AdventurerDeckZone");
            CardView[] _doc = _cardBuilder.BuildManySame(33);

            foreach (CardView card in _doc)
            {
                yield return sut.MoveCard(card).WaitForCompletion();
            }

            yield return new WaitForSeconds(150);
            Assert.That(_doc.First().transform.parent, Is.EqualTo(sut.transform));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Discard()
        {
            ZoneView sut = _zonesManager.Get("AdventurerDiscardZone");
            CardView[] _doc = _cardBuilder.BuildManySame(33);

            foreach (CardView card in _doc)
            {
                yield return sut.MoveCard(card).WaitForCompletion();
            }

            yield return new WaitForSeconds(150);
            Assert.That(_doc.First().transform.parent, Is.EqualTo(sut.transform));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Card()
        {
            ZoneView docZone = _zonesManager.Get("PlaceZone");
            CardView oneCard = _cardBuilder.BuildOne();
            ZoneCardView sut = oneCard.OwnZone;
            CardView[] _doc = _cardBuilder.BuildManySame(2);

            yield return docZone.MoveCard(oneCard).WaitForCompletion();


            foreach (CardView card in _doc)
            {
                yield return sut.MoveCard(card).WaitForCompletion();
            }

            yield return new WaitForSeconds(150);
            Assert.That(_doc[0].transform.parent, Is.EqualTo(sut.transform));
        }
    }
}
