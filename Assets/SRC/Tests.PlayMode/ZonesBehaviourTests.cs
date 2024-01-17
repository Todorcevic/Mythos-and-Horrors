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
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly CardViewsManager _cardViewsManager;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnitySetUp]
        public override IEnumerator SetUp()
        {
            ViewValues.FAST_TIME_ANIMATION = DEBUG_MODE ? ViewValues.FAST_TIME_ANIMATION : 0f;
            ViewValues.DEFAULT_TIME_ANIMATION = DEBUG_MODE ? ViewValues.DEFAULT_TIME_ANIMATION : 0f;

            yield return base.SetUp();
            _prepareGameUseCase.Execute();
        }

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Move_Card_In_Two_Zones()
        {
            CardView sut = _cardViewsManager.Get(_investigatorsProvider.Leader.InvestigatorCard);
            CardView sut2 = _cardViewsManager.Get(_investigatorsProvider.Leader.FullDeck[0]);

            yield return _gameActionFactory.Create<MoveCardsGameAction>().Run(sut.Card, _chaptersProvider.CurrentScene.PlaceZone[0, 2]).AsCoroutine();
            yield return _gameActionFactory.Create<MoveCardsGameAction>().Run(sut2.Card, _chaptersProvider.CurrentScene.PlaceZone[0, 3]).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.CurrentZoneView.Zone, Is.EqualTo(_chaptersProvider.CurrentScene.PlaceZone[0, 2]));
            Assert.That(sut.CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(sut));
            Assert.That(sut2.CurrentZoneView.Zone, Is.EqualTo(_chaptersProvider.CurrentScene.PlaceZone[0, 3]));
            Assert.That(sut2.CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(sut2));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Basic()
        {
            CardView sut = _cardViewsManager.Get(_investigatorsProvider.Leader.InvestigatorCard);

            yield return _gameActionFactory.Create<MoveCardsGameAction>().Run(sut.Card, _investigatorsProvider.Leader.InvestigatorZone).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.CurrentZoneView.Zone, Is.EqualTo(_investigatorsProvider.Leader.InvestigatorZone));
            Assert.That(sut.CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(sut));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Out()
        {
            CardView sut = _cardViewsManager.Get(_investigatorsProvider.Leader.InvestigatorCard);

            yield return _gameActionFactory.Create<MoveCardsGameAction>().Run(sut.Card, _chaptersProvider.CurrentScene.OutZone).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.CurrentZoneView.Zone, Is.EqualTo(_chaptersProvider.CurrentScene.OutZone));
            Assert.That(sut.CurrentZoneView.GetComponentsInChildren<CardView>(includeInactive: true), Contains.Item(sut));
            Assert.That(sut.gameObject.activeSelf, Is.EqualTo(false));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Row()
        {
            CardView[] sut = _investigatorsProvider.Leader.FullDeck.GetRange(0, 5).Select(card => _cardViewsManager.Get(card)).ToArray();

            yield return _gameActionFactory.Create<MoveCardsGameAction>().Run(sut.Select(cardView => cardView.Card).ToList(), _investigatorsProvider.Leader.AidZone).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.First().CurrentZoneView.Zone, Is.EqualTo(_investigatorsProvider.Leader.AidZone));
            Assert.That(sut.First().CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(sut.First()));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Row_Creating_Holders()
        {
            CardView[] sut = _investigatorsProvider.Leader.FullDeck.GetRange(0, 13).Select(card => _cardViewsManager.Get(card)).ToArray();

            foreach (CardView cardView in sut)
                yield return _gameActionFactory.Create<MoveCardsGameAction>().Run(cardView.Card, _investigatorsProvider.Leader.AidZone).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.First().CurrentZoneView.Zone, Is.EqualTo(_investigatorsProvider.Leader.AidZone));
            Assert.That(sut.First().CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(sut.First()));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Hand()
        {
            CardView[] sut = _investigatorsProvider.Leader.FullDeck.GetRange(0, 12).Select(card => _cardViewsManager.Get(card)).ToArray();

            yield return _gameActionFactory.Create<MoveCardsGameAction>().Run(sut.Select(cardView => cardView.Card).ToList(), _investigatorsProvider.Leader.HandZone).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.First().CurrentZoneView.Zone, Is.EqualTo(_investigatorsProvider.Leader.HandZone));
            Assert.That(sut.First().CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(sut.First()));
        }

        [UnityTest]
        public IEnumerator Remove_Card_In_Zone_Hand()
        {
            CardView[] sut = _investigatorsProvider.Leader.FullDeck.GetRange(0, 5).Select(card => _cardViewsManager.Get(card)).ToArray();

            yield return _gameActionFactory.Create<MoveCardsGameAction>().Run(sut.Select(cardView => cardView.Card).ToList(), _investigatorsProvider.Leader.HandZone).AsCoroutine();
            yield return _gameActionFactory.Create<MoveCardsGameAction>().Run(sut.First().Card, _chaptersProvider.CurrentScene.OutZone).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.First().CurrentZoneView.Zone, Is.EqualTo(_chaptersProvider.CurrentScene.OutZone));
        }

        [UnityTest]
        public IEnumerator Remove_Card_In_Zone_Row()
        {
            CardView[] sut = _investigatorsProvider.Leader.FullDeck.GetRange(0, 5).Select(card => _cardViewsManager.Get(card)).ToArray();

            yield return _gameActionFactory.Create<MoveCardsGameAction>().Run(sut.Select(cardView => cardView.Card).ToList(), _investigatorsProvider.Leader.AidZone).AsCoroutine();
            yield return _gameActionFactory.Create<MoveCardsGameAction>().Run(sut.First().Card, _chaptersProvider.CurrentScene.OutZone).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.First().CurrentZoneView.Zone, Is.EqualTo(_chaptersProvider.CurrentScene.OutZone));
        }

        [UnityTest]
        public IEnumerator Remove_Card_In_Zone_Deck()
        {
            CardView[] sut = _investigatorsProvider.Leader.FullDeck.Select(card => _cardViewsManager.Get(card)).ToArray();

            yield return _gameActionFactory.Create<MoveCardsGameAction>().Run(sut.Select(cardView => cardView.Card).ToList(), _investigatorsProvider.Leader.DeckZone).AsCoroutine();
            yield return _gameActionFactory.Create<MoveCardsGameAction>().Run(sut.Last().Card, _chaptersProvider.CurrentScene.OutZone).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.Last().CurrentZoneView.Zone, Is.EqualTo(_chaptersProvider.CurrentScene.OutZone));
        }

        [UnityTest]
        public IEnumerator Remove_Card_In_Zone_Discard()
        {
            CardView[] sut = _investigatorsProvider.Leader.FullDeck.Select(card => _cardViewsManager.Get(card)).ToArray();

            yield return _gameActionFactory.Create<MoveCardsGameAction>().Run(sut.Select(cardView => cardView.Card).ToList(), _investigatorsProvider.Leader.DiscardZone).AsCoroutine();
            yield return _gameActionFactory.Create<MoveCardsGameAction>().Run(sut.Last().Card, _chaptersProvider.CurrentScene.OutZone).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.Last().CurrentZoneView.Zone, Is.EqualTo(_chaptersProvider.CurrentScene.OutZone));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Deck()
        {
            CardView[] sut = _investigatorsProvider.Leader.FullDeck.Select(card => _cardViewsManager.Get(card)).ToArray();

            yield return _gameActionFactory.Create<MoveCardsGameAction>().Run(sut.Select(cardView => cardView.Card).ToList(), _investigatorsProvider.Leader.DeckZone).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.First().CurrentZoneView.Zone, Is.EqualTo(_investigatorsProvider.Leader.DeckZone));
            Assert.That(sut.First().CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(sut.First()));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Discard()
        {
            CardView[] sut = _investigatorsProvider.Leader.FullDeck.Select(card => _cardViewsManager.Get(card)).ToArray();

            yield return _gameActionFactory.Create<MoveCardsGameAction>().Run(sut.Select(cardView => cardView.Card).ToList(), _investigatorsProvider.Leader.DiscardZone).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.First().CurrentZoneView.Zone, Is.EqualTo(_investigatorsProvider.Leader.DiscardZone));
            Assert.That(sut.First().CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(sut.First()));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Card()
        {
            CardView sut2 = _cardViewsManager.Get(_investigatorsProvider.Leader.InvestigatorCard);
            CardView[] sut = _investigatorsProvider.Leader.FullDeck.GetRange(0, 4).Select(card => _cardViewsManager.Get(card)).ToArray();

            yield return _gameActionFactory.Create<MoveCardsGameAction>().Run(sut2.Card, _investigatorsProvider.Leader.AidZone).AsCoroutine();
            yield return _gameActionFactory.Create<MoveCardsGameAction>().Run(sut.Select(cardView => cardView.Card).ToList(), sut2.Card.OwnZone).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.First().CurrentZoneView.Zone, Is.EqualTo(sut2.Card.OwnZone));
            Assert.That(sut2.OwnZone.GetComponentsInChildren<CardView>(), Contains.Item(sut.First()));
        }

        [UnityTest]
        public IEnumerator Remove_Card_In_Zone_Card()
        {
            CardView sut2 = _cardViewsManager.Get(_investigatorsProvider.Leader.InvestigatorCard);
            CardView[] sut = _investigatorsProvider.Leader.FullDeck.GetRange(0, 4).Select(card => _cardViewsManager.Get(card)).ToArray();

            yield return _gameActionFactory.Create<MoveCardsGameAction>().Run(sut2.Card, _chaptersProvider.CurrentScene.PlaceZone[0, 2]).AsCoroutine();
            yield return _gameActionFactory.Create<MoveCardsGameAction>().Run(sut.Select(cardView => cardView.Card).ToList(), sut2.Card.OwnZone).AsCoroutine();
            yield return _gameActionFactory.Create<MoveCardsGameAction>().Run(sut.First().Card, _chaptersProvider.CurrentScene.OutZone).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.First().CurrentZoneView.Zone, Is.EqualTo(_chaptersProvider.CurrentScene.OutZone));
            Assert.That(sut2.OwnZone.GetComponentsInChildren<CardView>().Contains(sut.First()), Is.False);
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Out_And_Back()
        {
            CardView sut = _cardViewsManager.Get(_investigatorsProvider.Leader.InvestigatorCard);

            Assert.That(sut.gameObject.activeSelf, Is.EqualTo(false));
            yield return _gameActionFactory.Create<MoveCardsGameAction>().Run(sut.Card, _investigatorsProvider.Leader.HandZone).AsCoroutine();
            Assert.That(sut.gameObject.activeSelf, Is.EqualTo(true));
            yield return _gameActionFactory.Create<MoveCardsGameAction>().Run(sut.Card, _chaptersProvider.CurrentScene.OutZone).AsCoroutine();
            Assert.That(sut.gameObject.activeSelf, Is.EqualTo(false));
            yield return _gameActionFactory.Create<MoveCardsGameAction>().Run(sut.Card, _investigatorsProvider.Leader.HandZone).AsCoroutine();
            Assert.That(sut.gameObject.activeSelf, Is.EqualTo(true));

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.CurrentZoneView.Zone, Is.EqualTo(_investigatorsProvider.Leader.HandZone));
            Assert.That(sut.CurrentZoneView.GetComponentsInChildren<CardView>(includeInactive: true), Contains.Item(sut));
        }
    }
}
