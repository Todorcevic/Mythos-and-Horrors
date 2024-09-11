using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using TMPro;

namespace MythosAndHorrors.PlayModeView.Tests
{
    [TestFixture]
    public class MoveCardPresenterTests : PlayModeTestsBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Move_Card_To_Other_Investigator()
        {
            Investigator investigator1 = _investigatorsProvider.First;
            Investigator investigator2 = _investigatorsProvider.Second;
            Card card = investigator1.FullDeck[1];

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(card, investigator1.DangerZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(card, investigator2.DangerZone).Execute().AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_cardViewsManager.GetCardView(card).CurrentZoneView, Is.EqualTo(_zoneViewsManager.Get(investigator2.DangerZone)));
            Assert.That(_zoneViewsManager.Get(investigator2.DangerZone).GetComponentsInChildren<CardView>().Contains(_cardViewsManager.GetCardView(card)));
            Assert.That(_swapInvestigatorPresenter.GetPrivateMember<Investigator>("_investigatorSelected"), Is.EqualTo(investigator2));
        }

        [UnityTest]
        public IEnumerator Move_AvatarCard()
        {
            Investigator investigator1 = _investigatorsProvider.First;
            CardPlace cardPlace = _chaptersProvider.CurrentScene.PlaceCards.First();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardPlace, _chaptersProvider.CurrentScene.GetPlaceZone(1, 1)).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.First, cardPlace).Execute().AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_cardViewsManager.GetAvatarCardView(_investigatorsProvider.First).CurrentZoneView, Is.EqualTo(_zoneViewsManager.Get(cardPlace.OwnZone)));
        }

        [UnityTest]
        public IEnumerator Show_Specific_Card()
        {
            yield return BuildCard("01515", _investigatorsProvider.First);
            Card specificCard = _cardsProvider.GetCard<Card01502>();
            CardView cardView = _cardViewsManager.GetCardView(specificCard);
            string viewDescription = cardView.GetComponentInChildren<DescriptionController>().GetPrivateMember<TextMeshPro>("_description").text;
            cardView.MoveToZone(_zoneViewsManager.CenterShowZone);

            if (DEBUG_MODE) yield return PressAnyKey();
            if (DEBUG_MODE) yield return PressAnyKey();
            if (DEBUG_MODE) yield return PressAnyKey();
            if (DEBUG_MODE) yield return PressAnyKey();
            Assert.That(true);
        }
    }
}
