using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    [TestFixture]
    public class MoveCardPresenterTests : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ZoneViewsManager _zoneViewsManager;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly SwapInvestigatorHandler _swapInvestigatorPresenter;
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly ShowHistoryComponent _showHistoryComponent;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Move_Card_To_Other_Investigator()
        {
            _prepareGameUseCase.Execute();
            Investigator investigator1 = _investigatorsProvider.Leader;
            Investigator investigator2 = _investigatorsProvider.Second;
            Card card = investigator1.Cards[1];

            yield return _gameActionFactory.Create(new MoveCardsGameAction(card, investigator1.DangerZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(card, investigator2.DangerZone)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(_cardViewsManager.GetCardView(card).CurrentZoneView, Is.EqualTo(_zoneViewsManager.Get(investigator2.DangerZone)));
            Assert.That(_zoneViewsManager.Get(investigator2.DangerZone).GetComponentsInChildren<CardView>().Contains(_cardViewsManager.GetCardView(card)));
            Assert.That(_swapInvestigatorPresenter.GetPrivateMember<Investigator>("_investigatorSelected"), Is.EqualTo(investigator2));
        }

        [UnityTest]
        public IEnumerator Move_AvatarCard()
        {
            _prepareGameUseCase.Execute();
            Investigator investigator1 = _investigatorsProvider.Leader;
            CardPlace cardPlace = _chaptersProvider.CurrentScene.Info.PlaceCards[0];
            yield return _gameActionFactory.Create(new MoveCardsGameAction(cardPlace, investigator1.InvestigatorZone)).AsCoroutine();
            WaitToClickHistoryPanel().AsTask();

            yield return _gameActionFactory.Create(new MoveToPlaceGameAction(_investigatorsProvider.Leader, cardPlace)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(_cardViewsManager.GetAvatarCardView(_investigatorsProvider.Leader).CurrentZoneView, Is.EqualTo(_zoneViewsManager.Get(cardPlace.OwnZone)));
        }
    }
}
