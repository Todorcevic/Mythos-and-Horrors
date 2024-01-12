using MythsAndHorrors.GameRules;
using MythsAndHorrors.GameView;
using NUnit.Framework;
using System;
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
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly CardBuilder _cardBuilder;
        [Inject] private readonly CardViewBuilder _cardViewBuilder;
        [Inject] private readonly CardMoverPresenter _cardMoverPresenter;
        [Inject] private readonly ZoneLoaderUseCase _zoneLoaderUseCase;

        /*******************************************************************/
        [UnitySetUp]
        public override IEnumerator SetUp()
        {
            //DEBUG_MODE = true;
            ViewValues.FAST_TIME_ANIMATION = DEBUG_MODE ? ViewValues.FAST_TIME_ANIMATION : 0f;
            ViewValues.DEFAULT_TIME_ANIMATION = DEBUG_MODE ? ViewValues.DEFAULT_TIME_ANIMATION : 0f;

            yield return base.SetUp();
            _investigatorsProvider.AddInvestigator(new Investigator() { InvestigatorCard = _cardBuilder.BuildOfType<CardInvestigator>() });
            _chaptersProvider.SetCurrentScene(new SceneCORE1());
            _zoneLoaderUseCase.Execute();
        }

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Move_Card_In_Two_Zones()
        {
            CardView sut = _cardViewBuilder.BuildRand();
            CardView sut2 = _cardViewBuilder.BuildRand();

            yield return _cardMoverPresenter.RealMove(sut.Card, _chaptersProvider.CurrentScene.PlaceZone[0, 2]).AsCoroutine();
            yield return _cardMoverPresenter.RealMove(sut2.Card, _chaptersProvider.CurrentScene.PlaceZone[0, 3]).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.CurrentZoneView.Zone, Is.EqualTo(_chaptersProvider.CurrentScene.PlaceZone[0, 2]));
            Assert.That(sut.CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(sut));
            Assert.That(sut2.CurrentZoneView.Zone, Is.EqualTo(_chaptersProvider.CurrentScene.PlaceZone[0, 3]));
            Assert.That(sut2.CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(sut2));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Basic()
        {
            CardView sut = _cardViewBuilder.BuildRand();

            yield return _cardMoverPresenter.RealMove(sut.Card, _investigatorsProvider.Leader.InvestigatorZone).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.CurrentZoneView.Zone, Is.EqualTo(_investigatorsProvider.Leader.InvestigatorZone));
            Assert.That(sut.CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(sut));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Out()
        {
            CardView sut = _cardViewBuilder.BuildRand();

            yield return _cardMoverPresenter.RealMove(sut.Card, _chaptersProvider.CurrentScene.OutZone).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.CurrentZoneView.Zone, Is.EqualTo(_chaptersProvider.CurrentScene.OutZone));
            Assert.That(sut.CurrentZoneView.GetComponentsInChildren<CardView>(includeInactive: true), Contains.Item(sut));
            Assert.That(sut.gameObject.activeSelf, Is.EqualTo(false));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Row()
        {
            CardView[] sut = _cardViewBuilder.BuildManyRandom(5);

            foreach (CardView card in sut)
            {
                yield return _cardMoverPresenter.RealMove(card.Card, _investigatorsProvider.Leader.AidZone).AsCoroutine();
            }

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.First().CurrentZoneView.Zone, Is.EqualTo(_investigatorsProvider.Leader.AidZone));
            Assert.That(sut.First().CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(sut.First()));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Row_Creating_Holders()
        {
            CardView[] sut = _cardViewBuilder.BuildManyRandom(13);

            foreach (CardView card in sut)
            {
                yield return _cardMoverPresenter.RealMove(card.Card, _investigatorsProvider.Leader.AidZone).AsCoroutine();
            }

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.First().CurrentZoneView.Zone, Is.EqualTo(_investigatorsProvider.Leader.AidZone));
            Assert.That(sut.First().CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(sut.First()));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Hand()
        {
            CardView[] sut = _cardViewBuilder.BuildManyRandom(12);

            foreach (CardView card in sut)
            {
                yield return _cardMoverPresenter.RealMove(card.Card, _investigatorsProvider.Leader.HandZone).AsCoroutine();
            }

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.First().CurrentZoneView.Zone, Is.EqualTo(_investigatorsProvider.Leader.HandZone));
            Assert.That(sut.First().CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(sut.First()));
        }

        [UnityTest]
        public IEnumerator Remove_Card_In_Zone_Hand()
        {
            CardView[] sut = _cardViewBuilder.BuildManyRandom(5);

            foreach (CardView card in sut)
            {
                yield return _cardMoverPresenter.RealMove(card.Card, _investigatorsProvider.Leader.HandZone).AsCoroutine();
            }
            yield return _cardMoverPresenter.RealMove(sut.First().Card, _chaptersProvider.CurrentScene.OutZone).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.First().CurrentZoneView.Zone, Is.EqualTo(_chaptersProvider.CurrentScene.OutZone));
        }

        [UnityTest]
        public IEnumerator Remove_Card_In_Zone_Row()
        {
            CardView[] sut = _cardViewBuilder.BuildManyRandom(5);

            foreach (CardView card in sut)
            {
                yield return _cardMoverPresenter.RealMove(card.Card, _investigatorsProvider.Leader.AidZone).AsCoroutine();
            }
            yield return _cardMoverPresenter.RealMove(sut.First().Card, _chaptersProvider.CurrentScene.OutZone).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.First().CurrentZoneView.Zone, Is.EqualTo(_chaptersProvider.CurrentScene.OutZone));
        }

        [UnityTest]
        public IEnumerator Remove_Card_In_Zone_Deck()
        {
            CardView[] sut = _cardViewBuilder.BuildManyRandom(33);

            foreach (CardView card in sut)
            {
                yield return _cardMoverPresenter.RealMove(card.Card, _investigatorsProvider.Leader.DeckZone).AsCoroutine();
            }
            yield return _cardMoverPresenter.RealMove(sut.Last().Card, _chaptersProvider.CurrentScene.OutZone).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.Last().CurrentZoneView.Zone, Is.EqualTo(_chaptersProvider.CurrentScene.OutZone));
        }

        [UnityTest]
        public IEnumerator Remove_Card_In_Zone_Discard()
        {
            CardView[] sut = _cardViewBuilder.BuildManyRandom(33);

            foreach (CardView card in sut)
            {
                yield return _cardMoverPresenter.RealMove(card.Card, _investigatorsProvider.Leader.DiscardZone).AsCoroutine();
            }
            yield return _cardMoverPresenter.RealMove(sut.Last().Card, _chaptersProvider.CurrentScene.OutZone).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.Last().CurrentZoneView.Zone, Is.EqualTo(_chaptersProvider.CurrentScene.OutZone));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Deck()
        {
            CardView[] sut = _cardViewBuilder.BuildManyRandom(33);

            foreach (CardView card in sut)
            {
                yield return _cardMoverPresenter.RealMove(card.Card, _investigatorsProvider.Leader.DeckZone).AsCoroutine();
            }

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.First().CurrentZoneView.Zone, Is.EqualTo(_investigatorsProvider.Leader.DeckZone));
            Assert.That(sut.First().CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(sut.First()));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Discard()
        {
            CardView[] sut = _cardViewBuilder.BuildManyRandom(33);

            foreach (CardView card in sut)
            {
                yield return _cardMoverPresenter.RealMove(card.Card, _investigatorsProvider.Leader.DiscardZone).AsCoroutine();
            }

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.First().CurrentZoneView.Zone, Is.EqualTo(_investigatorsProvider.Leader.DiscardZone));
            Assert.That(sut.First().CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(sut.First()));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Card()
        {
            CardView sut2 = _cardViewBuilder.BuildRand();
            CardView[] sut = _cardViewBuilder.BuildManyRandom(4);

            yield return _cardMoverPresenter.RealMove(sut2.Card, _investigatorsProvider.Leader.AidZone).AsCoroutine();

            foreach (CardView card in sut)
            {
                yield return _cardMoverPresenter.RealMove(card.Card, sut2.Card.OwnZone).AsCoroutine();
            }

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.First().CurrentZoneView.Zone, Is.EqualTo(sut2.Card.OwnZone));
            Assert.That(sut2.OwnZone.GetComponentsInChildren<CardView>(), Contains.Item(sut.First()));
        }

        [UnityTest]
        public IEnumerator Remove_Card_In_Zone_Card()
        {
            CardView sut2 = _cardViewBuilder.BuildRand();
            CardView[] sut = _cardViewBuilder.BuildManyRandom(4);

            yield return _cardMoverPresenter.RealMove(sut2.Card, _chaptersProvider.CurrentScene.PlaceZone[0, 2]).AsCoroutine();

            foreach (CardView card in sut)
            {
                yield return _cardMoverPresenter.RealMove(card.Card, sut2.Card.OwnZone).AsCoroutine();
            }

            yield return _cardMoverPresenter.RealMove(sut.First().Card, _chaptersProvider.CurrentScene.OutZone).AsCoroutine();


            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.First().CurrentZoneView.Zone, Is.EqualTo(_chaptersProvider.CurrentScene.OutZone));
            Assert.That(sut2.OwnZone.GetComponentsInChildren<CardView>().Contains(sut.First()), Is.False);
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Out_And_Back()
        {
            CardView sut = _cardViewBuilder.BuildRand();
            Assert.That(sut.gameObject.activeSelf, Is.EqualTo(false));

            yield return _cardMoverPresenter.RealMove(sut.Card, _investigatorsProvider.Leader.HandZone).AsCoroutine();
            Assert.That(sut.gameObject.activeSelf, Is.EqualTo(true));

            yield return _cardMoverPresenter.RealMove(sut.Card, _chaptersProvider.CurrentScene.OutZone).AsCoroutine();
            Assert.That(sut.gameObject.activeSelf, Is.EqualTo(false));

            yield return _cardMoverPresenter.RealMove(sut.Card, _investigatorsProvider.Leader.HandZone).AsCoroutine();
            Assert.That(sut.gameObject.activeSelf, Is.EqualTo(true));

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.CurrentZoneView.Zone, Is.EqualTo(_investigatorsProvider.Leader.HandZone));
            Assert.That(sut.CurrentZoneView.GetComponentsInChildren<CardView>(includeInactive: true), Contains.Item(sut));
        }
    }
}
