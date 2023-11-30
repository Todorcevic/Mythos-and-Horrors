using DG.Tweening;
using MythsAndHorrors.GameRules;
using MythsAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
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
        [Inject] private readonly CardMoverPresenter _cardMoverPresenter;

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
            CardView doc = _cardViewBuilder.BuildRand();
            CardView doc2 = _cardViewBuilder.BuildRand();

            yield return _cardMoverPresenter.MoveCardToZoneAsync(doc.Card, _zonesProvider.PlaceZone[0, 2]).AsCoroutine();
            yield return _cardMoverPresenter.MoveCardToZoneAsync(doc2.Card, _zonesProvider.PlaceZone[0, 3]).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(doc.CurrentZoneView.Zone, Is.EqualTo(_zonesProvider.PlaceZone[0, 2]));
            Assert.That(doc.CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(doc));
            Assert.That(doc2.CurrentZoneView.Zone, Is.EqualTo(_zonesProvider.PlaceZone[0, 3]));
            Assert.That(doc2.CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(doc2));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Basic()
        {
            CardView doc = _cardViewBuilder.BuildRand();

            yield return _cardMoverPresenter.MoveCardToZoneAsync(doc.Card, _adventurersProvider.Leader.AdventurerZone).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(doc.CurrentZoneView.Zone, Is.EqualTo(_adventurersProvider.Leader.AdventurerZone));
            Assert.That(doc.CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(doc));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Out()
        {
            CardView doc = _cardViewBuilder.BuildRand();

            yield return _cardMoverPresenter.MoveCardToZoneAsync(doc.Card, _zonesProvider.OutZone).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(doc.CurrentZoneView.Zone, Is.EqualTo(_zonesProvider.OutZone));
            Assert.That(doc.CurrentZoneView.GetComponentsInChildren<CardView>(includeInactive: true), Contains.Item(doc));
            Assert.That(doc.gameObject.activeSelf, Is.EqualTo(false));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Row()
        {
            CardView[] doc = _cardViewBuilder.BuildManyRandom(5);

            foreach (CardView card in doc)
            {
                yield return _cardMoverPresenter.MoveCardToZoneAsync(card.Card, _adventurersProvider.Leader.AidZone).AsCoroutine();
            }

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(doc.First().CurrentZoneView.Zone, Is.EqualTo(_adventurersProvider.Leader.AidZone));
            Assert.That(doc.First().CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(doc.First()));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Row_Creating_Holders()
        {
            CardView[] doc = _cardViewBuilder.BuildManyRandom(13);

            foreach (CardView card in doc)
            {
                yield return _cardMoverPresenter.MoveCardToZoneAsync(card.Card, _adventurersProvider.Leader.AidZone).AsCoroutine();
            }

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(doc.First().CurrentZoneView.Zone, Is.EqualTo(_adventurersProvider.Leader.AidZone));
            Assert.That(doc.First().CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(doc.First()));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Hand()
        {
            CardView[] doc = _cardViewBuilder.BuildManyRandom(12);

            foreach (CardView card in doc)
            {
                yield return _cardMoverPresenter.MoveCardToZoneAsync(card.Card, _adventurersProvider.Leader.HandZone).AsCoroutine();
            }

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(doc.First().CurrentZoneView.Zone, Is.EqualTo(_adventurersProvider.Leader.HandZone));
            Assert.That(doc.First().CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(doc.First()));
        }

        [UnityTest]
        public IEnumerator Remove_Card_In_Zone_Hand()
        {
            CardView[] doc = _cardViewBuilder.BuildManyRandom(5);

            foreach (CardView card in doc)
            {
                yield return _cardMoverPresenter.MoveCardToZoneAsync(card.Card, _adventurersProvider.Leader.HandZone).AsCoroutine();
            }
            yield return _cardMoverPresenter.MoveCardToZoneAsync(doc.First().Card, _zonesProvider.OutZone).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(doc.First().CurrentZoneView.Zone, Is.EqualTo(_zonesProvider.OutZone));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Deck()
        {
            CardView[] doc = _cardViewBuilder.BuildManyRandom(33);

            foreach (CardView card in doc)
            {
                yield return _cardMoverPresenter.MoveCardToZoneAsync(card.Card, _adventurersProvider.Leader.DeckZone).AsCoroutine();
            }

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(doc.First().CurrentZoneView.Zone, Is.EqualTo(_adventurersProvider.Leader.DeckZone));
            Assert.That(doc.First().CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(doc.First()));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Discard()
        {
            CardView[] doc = _cardViewBuilder.BuildManyRandom(33);

            foreach (CardView card in doc)
            {
                yield return _cardMoverPresenter.MoveCardToZoneAsync(card.Card, _adventurersProvider.Leader.DiscardZone).AsCoroutine();
            }

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(doc.First().CurrentZoneView.Zone, Is.EqualTo(_adventurersProvider.Leader.DiscardZone));
            Assert.That(doc.First().CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(doc.First()));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Card()
        {
            CardView oneCard = _cardViewBuilder.BuildRand();
            CardView[] doc = _cardViewBuilder.BuildManyRandom(4);

            yield return _cardMoverPresenter.MoveCardToZoneAsync(oneCard.Card, _adventurersProvider.Leader.AidZone).AsCoroutine();

            foreach (CardView card in doc)
            {
                yield return _cardMoverPresenter.MoveCardToZoneAsync(card.Card, oneCard.Card.OwnZone).AsCoroutine();
            }

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(doc.First().CurrentZoneView.Zone, Is.EqualTo(oneCard.Card.OwnZone));
            Assert.That(oneCard.OwnZone.GetComponentsInChildren<CardView>(), Contains.Item(doc.First()));
        }
    }
}
