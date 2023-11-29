using DG.Tweening;
using MythsAndHorrors.GameRules;
using MythsAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythsAndHorrors.PlayMode.Tests
{
    [TestFixture]
    public class ZonesBehaviourTests : TestBase
    {
        private readonly bool DEBUG_MODE = false;
        [Inject] private readonly ZoneViewsManager _zonesManager;
        [Inject] private readonly ZonesProvider _zonesProvider;
        [Inject] private readonly AdventurersProvider _adventurersProvider;
        [Inject] private readonly CardBuilder _cardBuilder;
        [Inject] private readonly CardViewBuilder _cardViewBuilder;

        /*******************************************************************/
        [UnitySetUp]
        public override IEnumerator SetUp()
        {
            ViewValues.FAST_TIME_ANIMATION = DEBUG_MODE ? ViewValues.FAST_TIME_ANIMATION : 0f;

            yield return base.SetUp();
            _adventurersProvider.AddAdventurer(new Adventurer() { AdventurerCard = _cardBuilder.BuildOfType<CardAdventurer>() });
            _zonesManager.Init();
        }

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Move_Card_In_Two_Zones()
        {
            ZoneView sut = _zonesManager.Get(_zonesProvider.PlaceZone[0, 2]);
            CardView doc = _cardViewBuilder.BuildRand();

            ZoneView sut2 = _zonesManager.Get(_zonesProvider.PlaceZone[0, 3]);
            CardView doc2 = _cardViewBuilder.BuildRand();

            yield return sut.EnterCard(doc).WaitForCompletion();
            yield return sut2.EnterCard(doc2).WaitForCompletion();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(doc.transform.parent, Is.EqualTo(sut.transform));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Basic()
        {
            ZoneView sut = _zonesManager.Get(_adventurersProvider.Leader.AdventurerZone);
            CardView doc = _cardViewBuilder.BuildRand();

            yield return sut.EnterCard(doc).WaitForCompletion();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(doc.transform.parent, Is.EqualTo(sut.transform));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Row()
        {
            ZoneView sut = _zonesManager.Get(_adventurersProvider.Leader.AidZone);
            CardView[] doc = _cardViewBuilder.BuildManyRandom(5);

            foreach (CardView card in doc)
            {
                yield return sut.EnterCard(card).WaitForCompletion();
            }

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(doc.First().transform.parent, Is.EqualTo(sut.transform));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Row_Creating_Holders()
        {
            ZoneView sut = _zonesManager.Get(_adventurersProvider.Leader.AidZone);
            CardView[] doc = _cardViewBuilder.BuildManyRandom(13);

            foreach (CardView card in doc)
            {
                yield return sut.EnterCard(card).WaitForCompletion();
            }

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(doc.First().transform.parent, Is.EqualTo(sut.transform));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Hand()
        {
            ZoneView sut = _zonesManager.Get(_adventurersProvider.Leader.HandZone);
            CardView[] doc = _cardViewBuilder.BuildManyRandom(12);

            foreach (CardView card in doc)
            {
                yield return sut.EnterCard(card).WaitForCompletion();
            }

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(doc.First().transform.parent, Is.EqualTo(sut.transform));
        }

        [UnityTest]
        public IEnumerator Remove_Card_In_Zone_Hand()
        {
            ZoneView sut = _zonesManager.Get(_adventurersProvider.Leader.HandZone);
            ZoneView doc2 = _zonesManager.Get(_zonesProvider.OutZone);
            CardView[] doc = _cardViewBuilder.BuildManyRandom(5);

            foreach (CardView card in doc)
            {
                yield return sut.EnterCard(card).WaitForCompletion();
            }

            yield return doc2.EnterCard(doc[0]);
            yield return sut.ExitCard(doc[0]);


            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(doc.First().transform.parent, Is.Not.EqualTo(sut.transform));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Deck()
        {
            ZoneView sut = _zonesManager.Get(_adventurersProvider.Leader.DeckZone);
            CardView[] doc = _cardViewBuilder.BuildManyRandom(33);

            foreach (CardView card in doc)
            {
                yield return sut.EnterCard(card).WaitForCompletion();
            }

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(doc.First().transform.parent, Is.EqualTo(sut.transform));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Discard()
        {
            ZoneView sut = _zonesManager.Get(_adventurersProvider.Leader.DiscardZone);
            CardView[] doc = _cardViewBuilder.BuildManyRandom(33);

            foreach (CardView card in doc)
            {
                yield return sut.EnterCard(card).WaitForCompletion();
            }

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(doc.First().transform.parent, Is.EqualTo(sut.transform));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Card()
        {
            ZoneView docZone = _zonesManager.Get(_adventurersProvider.Leader.AidZone);
            CardView oneCard = _cardViewBuilder.BuildRand();
            ZoneCardView sut = oneCard.OwnZone;
            CardView[] doc = _cardViewBuilder.BuildManyRandom(4);

            yield return docZone.EnterCard(oneCard).WaitForCompletion();

            foreach (CardView card in doc)
            {
                yield return sut.EnterCard(card).WaitForCompletion();
            }

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(doc[0].transform.parent, Is.EqualTo(sut.transform));
        }
    }
}
