using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeView.Tests
{
    [TestFixture]
    public class ZonesBehaviourTests : PlayModeTestsBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Move_Card_In_Two_Zones()
        {
            CardView sut = _cardViewsManager.GetCardView(_investigatorsProvider.First.InvestigatorCard);
            CardView sut2 = _cardViewsManager.GetCardView(_investigatorsProvider.First.FullDeck.First());

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(sut.Card, _chaptersProvider.CurrentScene.PlaceZone[0, 2])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(sut2.Card, _chaptersProvider.CurrentScene.PlaceZone[0, 3])).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.CurrentZoneView.Zone, Is.EqualTo(_chaptersProvider.CurrentScene.PlaceZone[0, 2]));
            Assert.That(sut.CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(sut));
            Assert.That(sut2.CurrentZoneView.Zone, Is.EqualTo(_chaptersProvider.CurrentScene.PlaceZone[0, 3]));
            Assert.That(sut2.CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(sut2));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Basic()
        {
            CardView sut = _cardViewsManager.GetCardView(_investigatorsProvider.First.InvestigatorCard);

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(sut.Card, _investigatorsProvider.First.InvestigatorZone)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.CurrentZoneView.Zone, Is.EqualTo(_investigatorsProvider.First.InvestigatorZone));
            Assert.That(sut.CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(sut));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Out()
        {
            CardView sut = _cardViewsManager.GetCardView(_investigatorsProvider.First.InvestigatorCard);

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(sut.Card, _chaptersProvider.CurrentScene.OutZone)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.CurrentZoneView.Zone, Is.EqualTo(_chaptersProvider.CurrentScene.OutZone));
            Assert.That(sut.CurrentZoneView.GetComponentsInChildren<CardView>(includeInactive: true), Contains.Item(sut));
            Assert.That(sut.gameObject.activeSelf, Is.EqualTo(false));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Row()
        {
            CardView[] sut = _investigatorsProvider.First.FullDeck.Take(5).Select(card => _cardViewsManager.GetCardView(card)).ToArray();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(sut.Select(cardView => cardView.Card).ToList(), _investigatorsProvider.First.AidZone)).AsCoroutine();

            _ioActivatorComponent.ActivateCardSensors();
            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.First().CurrentZoneView.Zone, Is.EqualTo(_investigatorsProvider.First.AidZone));
            Assert.That(sut.First().CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(sut.First()));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Row_Creating_Holders()
        {
            CardView[] sut = _investigatorsProvider.First.FullDeck.Take(13).Select(card => _cardViewsManager.GetCardView(card)).ToArray();

            foreach (CardView cardView in sut)
                yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardView.Card, _investigatorsProvider.First.AidZone)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.First().CurrentZoneView.Zone, Is.EqualTo(_investigatorsProvider.First.AidZone));
            Assert.That(sut.First().CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(sut.First()));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Hand()
        {
            CardView[] sut = _investigatorsProvider.First.FullDeck.Take(5).Select(card => _cardViewsManager.GetCardView(card)).ToArray();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(sut.Select(cardView => cardView.Card).ToList(), _investigatorsProvider.First.HandZone)).AsCoroutine();

            _ioActivatorComponent.ActivateCardSensors();
            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.First().CurrentZoneView.Zone, Is.EqualTo(_investigatorsProvider.First.HandZone));
            Assert.That(sut.First().CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(sut.First()));
        }

        [UnityTest]
        public IEnumerator Remove_Card_In_Zone_Hand()
        {
            CardView[] sut = _investigatorsProvider.First.FullDeck.Take(5).Select(card => _cardViewsManager.GetCardView(card)).ToArray();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(sut.Select(cardView => cardView.Card).ToList(), _investigatorsProvider.First.HandZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(sut.First().Card, _chaptersProvider.CurrentScene.OutZone)).AsCoroutine();

            _ioActivatorComponent.ActivateCardSensors();
            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.First().CurrentZoneView.Zone, Is.EqualTo(_chaptersProvider.CurrentScene.OutZone));
        }

        [UnityTest]
        public IEnumerator Remove_Card_In_Zone_Row()
        {
            CardView[] sut = _investigatorsProvider.First.FullDeck.Take(5).Select(card => _cardViewsManager.GetCardView(card)).ToArray();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(sut.Select(cardView => cardView.Card).ToList(), _investigatorsProvider.First.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(sut.First().Card, _chaptersProvider.CurrentScene.OutZone)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.First().CurrentZoneView.Zone, Is.EqualTo(_chaptersProvider.CurrentScene.OutZone));
        }

        [UnityTest]
        public IEnumerator Remove_Card_In_Zone_Deck()
        {
            CardView[] sut = _investigatorsProvider.First.FullDeck.Select(card => _cardViewsManager.GetCardView(card)).ToArray();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(sut.Select(cardView => cardView.Card).ToList(), _investigatorsProvider.First.DeckZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(sut.Last().Card, _chaptersProvider.CurrentScene.OutZone)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.Last().CurrentZoneView.Zone, Is.EqualTo(_chaptersProvider.CurrentScene.OutZone));
        }

        [UnityTest]
        public IEnumerator Remove_Card_In_Zone_Discard()
        {
            CardView[] sut = _investigatorsProvider.First.FullDeck.Select(card => _cardViewsManager.GetCardView(card)).ToArray();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(sut.Select(cardView => cardView.Card).ToList(), _investigatorsProvider.First.DiscardZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(sut.Last().Card, _chaptersProvider.CurrentScene.OutZone)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.Last().CurrentZoneView.Zone, Is.EqualTo(_chaptersProvider.CurrentScene.OutZone));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Deck()
        {
            CardView[] sut = _investigatorsProvider.First.FullDeck.Select(card => _cardViewsManager.GetCardView(card)).ToArray();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(sut.Select(cardView => cardView.Card).ToList(), _investigatorsProvider.First.DeckZone)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.First().CurrentZoneView.Zone, Is.EqualTo(_investigatorsProvider.First.DeckZone));
            Assert.That(sut.First().CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(sut.First()));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Discard()
        {
            CardView[] sut = _investigatorsProvider.First.FullDeck.Select(card => _cardViewsManager.GetCardView(card)).ToArray();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(sut.Select(cardView => cardView.Card).ToList(), _investigatorsProvider.First.DiscardZone)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.First().CurrentZoneView.Zone, Is.EqualTo(_investigatorsProvider.First.DiscardZone));
            Assert.That(sut.First().CurrentZoneView.GetComponentsInChildren<CardView>(), Contains.Item(sut.First()));
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Card()
        {
            CardView sut2 = _cardViewsManager.GetCardView(_investigatorsProvider.First.InvestigatorCard);
            CardView[] sut = _investigatorsProvider.First.FullDeck.Take(4).Select(card => _cardViewsManager.GetCardView(card)).ToArray();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(sut2.Card, _investigatorsProvider.First.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(sut.Select(cardView => cardView.Card).ToList(), sut2.Card.OwnZone)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.First().CurrentZoneView.Zone, Is.EqualTo(sut2.Card.OwnZone));
            Assert.That(sut2.GetPrivateMember<ZoneCardView>("_ownZoneCardView").GetComponentsInChildren<CardView>(), Contains.Item(sut.First()));
        }

        [UnityTest]
        public IEnumerator Remove_Card_In_Zone_Card()
        {
            CardView sut2 = _cardViewsManager.GetCardView(_investigatorsProvider.First.InvestigatorCard);
            CardView[] sut = _investigatorsProvider.First.FullDeck.Take(4).Select(card => _cardViewsManager.GetCardView(card)).ToArray();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(sut2.Card, _chaptersProvider.CurrentScene.PlaceZone[0, 2])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(sut.Select(cardView => cardView.Card).ToList(), sut2.Card.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(sut.First().Card, _chaptersProvider.CurrentScene.OutZone)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.First().CurrentZoneView.Zone, Is.EqualTo(_chaptersProvider.CurrentScene.OutZone));
            Assert.That(sut2.GetPrivateMember<ZoneCardView>("_ownZoneCardView").GetComponentsInChildren<CardView>().Contains(sut.First()), Is.False);
        }

        [UnityTest]
        public IEnumerator Move_Card_In_Zone_Out_And_Back()
        {
            CardView sut = _cardViewsManager.GetCardView(_investigatorsProvider.First.InvestigatorCard);

            Assert.That(sut.gameObject.activeSelf, Is.EqualTo(false));
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(sut.Card, _investigatorsProvider.First.HandZone)).AsCoroutine();
            Assert.That(sut.gameObject.activeSelf, Is.EqualTo(true));
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(sut.Card, _chaptersProvider.CurrentScene.OutZone)).AsCoroutine();
            Assert.That(sut.gameObject.activeSelf, Is.EqualTo(false));
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(sut.Card, _investigatorsProvider.First.HandZone)).AsCoroutine();
            Assert.That(sut.gameObject.activeSelf, Is.EqualTo(true));

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(sut.CurrentZoneView.Zone, Is.EqualTo(_investigatorsProvider.First.HandZone));
            Assert.That(sut.CurrentZoneView.GetComponentsInChildren<CardView>(includeInactive: true), Contains.Item(sut));
        }
    }
}
