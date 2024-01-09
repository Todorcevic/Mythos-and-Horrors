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
    public class MoveCardPresenterTests : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUse;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly CardMoverPresenter _cardMoverPresenter;
        [Inject] private readonly ZoneViewsManager _zoneViewsManager;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly SwapInvestigatorComponent _swapInvestigatorComponent;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Move_Card_To_Other_Investigator()
        {
            //DEBUG_MODE = true;

            _prepareGameUse.Execute();
            Investigator investigator1 = _investigatorsProvider.Leader;
            Investigator investigator2 = _investigatorsProvider.Second;
            Card card = investigator1.Cards[1];

            yield return _cardMoverPresenter.MoveCardToZone(card, investigator1.AidZone).AsCoroutine();
            yield return _cardMoverPresenter.MoveCardToZone(card, investigator2.DangerZone).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(_cardViewsManager.Get(card).CurrentZoneView, Is.EqualTo(_zoneViewsManager.Get(investigator2.DangerZone)));
            Assert.That(_zoneViewsManager.Get(investigator2.DangerZone).GetComponentsInChildren<CardView>().Contains(_cardViewsManager.Get(card)));
            Assert.That(_swapInvestigatorComponent.InvestigatorSelected, Is.EqualTo(investigator2));
        }
    }
}
