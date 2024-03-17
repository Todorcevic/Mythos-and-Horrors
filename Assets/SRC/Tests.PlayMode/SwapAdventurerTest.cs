using DG.Tweening;
using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    [TestFixture]
    public class SwapInvestigatorTest : TestBase
    {
        [Inject] private readonly SwapInvestigatorComponent _sut;
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Swap()
        {
            _prepareGameUseCase.Execute();
            Investigator investigator1 = _investigatorsProvider.First;
            Investigator investigator2 = _investigatorsProvider.Second;

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(investigator1.InvestigatorCard, investigator1.InvestigatorZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(investigator1.FullDeck.ElementAt(1), investigator1.HandZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(investigator1.FullDeck.ElementAt(2), investigator1.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(investigator1.FullDeck.ElementAt(3), investigator1.DiscardZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(investigator1.FullDeck.ElementAt(4), investigator1.DeckZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(investigator1.FullDeck.ElementAt(5), investigator1.DangerZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(investigator2.InvestigatorCard, investigator2.InvestigatorZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(investigator2.FullDeck.ElementAt(1), investigator2.HandZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(investigator2.FullDeck.ElementAt(2), investigator2.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(investigator2.FullDeck.ElementAt(3), investigator2.DiscardZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(investigator2.FullDeck.ElementAt(4), investigator2.DeckZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(investigator2.FullDeck.ElementAt(5), investigator2.DangerZone)).AsCoroutine();

            if (DEBUG_MODE) Time.timeScale = 1;
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
