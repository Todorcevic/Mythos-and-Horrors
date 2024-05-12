using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    [TestFixture]
    public class MoveCardPresenterTests : TestCORE1PlayModeBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Move_Card_To_Other_Investigator()
        {
            Investigator investigator1 = _investigatorsProvider.First;
            Investigator investigator2 = _investigatorsProvider.Second;
            Card card = investigator1.Cards[1];

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(card, investigator1.DangerZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(card, investigator2.DangerZone)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_cardViewsManager.GetCardView(card).CurrentZoneView, Is.EqualTo(_zoneViewsManager.Get(investigator2.DangerZone)));
            Assert.That(_zoneViewsManager.Get(investigator2.DangerZone).GetComponentsInChildren<CardView>().Contains(_cardViewsManager.GetCardView(card)));
            Assert.That(_swapInvestigatorPresenter.GetPrivateMember<Investigator>("_investigatorSelected"), Is.EqualTo(investigator2));
        }

        [UnityTest]
        public IEnumerator Move_AvatarCard()
        {
            Investigator investigator1 = _investigatorsProvider.First;
            CardPlace cardPlace = _chaptersProvider.CurrentScene.Info.PlaceCards.First();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardPlace, investigator1.InvestigatorZone)).AsCoroutine();

            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.First, cardPlace)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_cardViewsManager.GetAvatarCardView(_investigatorsProvider.First).CurrentZoneView, Is.EqualTo(_zoneViewsManager.Get(cardPlace.OwnZone)));
        }
    }
}
