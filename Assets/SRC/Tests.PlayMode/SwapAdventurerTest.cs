using DG.Tweening;
using MythsAndHorrors.GameRules;
using MythsAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythsAndHorrors.PlayMode.Tests
{
    [TestFixture]
    public class SwapInvestigatorTest : TestBase
    {
        [Inject] private readonly SwapInvestigatorComponent _sut;
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly CardMoverPresenter _cardMoverPresenter;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Swap()
        {
            _prepareGameUseCase.Execute();
            Investigator investigator1 = _investigatorsProvider.AllInvestigators[0];
            Investigator investigator2 = _investigatorsProvider.AllInvestigators[1];

            if (DEBUG_MODE) ViewValues.FAST_TIME_ANIMATION = 0;
            yield return _cardMoverPresenter.MoveCardWithPreviewToZone(investigator1.InvestigatorCard, investigator1.HandZone).AsCoroutine();
            yield return _cardMoverPresenter.MoveCardWithPreviewToZone(investigator2.InvestigatorCard, investigator2.HandZone).AsCoroutine();
            yield return _cardMoverPresenter.MoveCardWithPreviewToZone(investigator1.Cards[2], investigator1.AidZone).AsCoroutine();
            yield return _cardMoverPresenter.MoveCardWithPreviewToZone(investigator2.Cards[2], investigator2.AidZone).AsCoroutine();

            yield return _cardMoverPresenter.MoveCardWithPreviewToZone(investigator1.Cards[3], investigator1.DiscardZone).AsCoroutine();
            yield return _cardMoverPresenter.MoveCardWithPreviewToZone(investigator2.Cards[3], investigator2.DiscardZone).AsCoroutine();

            yield return _cardMoverPresenter.MoveCardWithPreviewToZone(investigator1.Cards[4], investigator1.DeckZone).AsCoroutine();
            yield return _cardMoverPresenter.MoveCardWithPreviewToZone(investigator2.Cards[4], investigator2.DeckZone).AsCoroutine();

            yield return _cardMoverPresenter.MoveCardWithPreviewToZone(investigator1.Cards[5], investigator1.InvestigatorZone).AsCoroutine();
            yield return _cardMoverPresenter.MoveCardWithPreviewToZone(investigator2.Cards[5], investigator2.InvestigatorZone).AsCoroutine();

            yield return _cardMoverPresenter.MoveCardWithPreviewToZone(investigator1.Cards[6], investigator1.DangerZone).AsCoroutine();
            yield return _cardMoverPresenter.MoveCardWithPreviewToZone(investigator2.Cards[6], investigator2.DangerZone).AsCoroutine();

            if (DEBUG_MODE) ViewValues.FAST_TIME_ANIMATION = 0.25f;
            while (DEBUG_MODE)
            {
                yield return PressAnyKey();
                yield return _sut.Select(investigator2).WaitForCompletion();
                yield return PressAnyKey();
                yield return _sut.Select(investigator1).WaitForCompletion();
            }

            yield return _sut.Select(investigator2).WaitForCompletion();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(_sut.GetPrivateMember<AreaInvestigatorView>("_currentAreaInvestigator").Investigator, Is.EqualTo(investigator2));
        }
    }
}
