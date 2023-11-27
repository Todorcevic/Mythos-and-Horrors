using DG.Tweening;
using MythsAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythsAndHorrors.GameView.Tests
{
    [TestFixture]
    public class SwapAdventurerTest : TestBase
    {
        private readonly bool DEBUG_MODE = true;
        [Inject] private readonly AdventurersProvider _adventurersProvider;
        [Inject] private readonly ZoneViewsManager _zonesManager;
        [Inject] private readonly SwapAdventurerComponent _swapAdventurerComponent;
        [Inject] private readonly CardBuilder _cardBuilder;
        [Inject] private readonly CardViewGeneratorComponent _cardGenerator;

        /*******************************************************************/
        [UnitySetUp]
        public override IEnumerator SetUp()
        {
            yield return base.SetUp();
        }

        /*******************************************************************/

        [UnityTest]
        public IEnumerator Prepare_Full_Adventurer()
        {
            Adventurer adventurer1 = new Adventurer() { AdventurerCard = _cardBuilder.BraveCard };
            Adventurer adventurer2 = new Adventurer() { AdventurerCard = _cardBuilder.CunningCard };
            _adventurersProvider.AddAdventurer(adventurer1);
            _adventurersProvider.AddAdventurer(adventurer2);
            _zonesManager.Init();


            ZoneView adventurer1Zone = _zonesManager.Get(adventurer1.HandZone);
            CardView oneCard = _cardGenerator.BuildCard(adventurer1.AdventurerCard);
            yield return adventurer1Zone.EnterCard(oneCard).WaitForCompletion();

            ZoneView adventurer2Zone = _zonesManager.Get(adventurer2.HandZone);
            CardView twoCard = _cardGenerator.BuildCard(adventurer2.AdventurerCard);
            yield return adventurer2Zone.EnterCard(twoCard).WaitForCompletion();

            yield return PressAnyKey();
            yield return _swapAdventurerComponent.Select(adventurer2).WaitForCompletion();
            yield return PressAnyKey();
            yield return _swapAdventurerComponent.Select(adventurer1).WaitForCompletion();
            yield return PressAnyKey();
            yield return _swapAdventurerComponent.Select(adventurer2).WaitForCompletion();
            yield return PressAnyKey();
            yield return _swapAdventurerComponent.Select(adventurer1).WaitForCompletion();
            yield return PressAnyKey();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
        }


    }

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
            _adventurersProvider.AddAdventurer(new Adventurer() { AdventurerCard = _cardBuilder.BraveCard });
            _zonesManager.Init();
        }

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Basic()
        {
            ZoneView sut = _zonesManager.Get(_adventurersProvider.Leader.AdventurerZone);
            CardView doc = _cardViewBuilder.BuildOne();

            yield return sut.EnterCard(doc).WaitForCompletion();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(doc.transform.parent, Is.EqualTo(sut.transform));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Row()
        {
            ZoneView sut = _zonesManager.Get(_adventurersProvider.Leader.AidZone);
            CardView[] doc = _cardViewBuilder.BuildManySame(5);

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
            CardView[] doc = _cardViewBuilder.BuildManySame(13);

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
            CardView[] doc = _cardViewBuilder.BuildManySame(12);

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
            CardView[] doc = _cardViewBuilder.BuildManySame(5);

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
            CardView[] doc = _cardViewBuilder.BuildManySame(33);

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
            CardView[] doc = _cardViewBuilder.BuildManySame(33);

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
            ZoneView docZone = _zonesManager.Get(_zonesProvider.PlaceZone[1, 3]);
            CardView oneCard = _cardViewBuilder.BuildOne();
            ZoneCardView sut = oneCard.OwnZone;
            CardView[] doc = _cardViewBuilder.BuildManySame(4);

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
