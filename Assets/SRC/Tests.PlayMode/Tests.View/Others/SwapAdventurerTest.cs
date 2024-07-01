using DG.Tweening;
using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using Zenject;

namespace MythosAndHorrors.PlayModeView.Tests
{
    [TestFixture]
    public class SwapInvestigatorTest : PlayModeTestsBase
    {
        [Inject] private readonly SwapInvestigatorComponent _sut;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Swap()
        {
            Investigator investigator1 = _investigatorsProvider.First;
            Investigator investigator2 = _investigatorsProvider.Second;

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator1.InvestigatorCard, investigator1.InvestigatorZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator1.FullDeck.ElementAt(1), investigator1.HandZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator1.FullDeck.ElementAt(2), investigator1.AidZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator1.FullDeck.ElementAt(3), investigator1.DiscardZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator1.FullDeck.ElementAt(4), investigator1.DeckZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator1.FullDeck.ElementAt(5), investigator1.DangerZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator2.InvestigatorCard, investigator2.InvestigatorZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator2.FullDeck.ElementAt(1), investigator2.HandZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator2.FullDeck.ElementAt(2), investigator2.AidZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator2.FullDeck.ElementAt(3), investigator2.DiscardZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator2.FullDeck.ElementAt(4), investigator2.DeckZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator2.FullDeck.ElementAt(5), investigator2.DangerZone).Start().AsCoroutine();

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
