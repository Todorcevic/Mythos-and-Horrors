using DG.Tweening;
using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using System.Linq;

namespace MythosAndHorrors.PlayModeView.Tests
{
    [TestFixture]
    public class ResourcePileTests : PlayModeTestsBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Move_Resource_With_Handler()
        {
            yield return _tokenMoveHandler.GainResourceAnimation(_investigatorsProvider.First, 5, _chaptersProvider.CurrentScene.PileAmount)
                .WaitForCompletion();

            Assert.That(_areaInvestigatorViewsManager.Get(_investigatorsProvider.First).ResourcesTokenController.Amount, Is.EqualTo(5));

            yield return _tokenMoveHandler.PayResourceAnimation(_investigatorsProvider.First, 5, _chaptersProvider.CurrentScene.PileAmount)
               .WaitForCompletion();

            Assert.That(_areaInvestigatorViewsManager.Get(_investigatorsProvider.First).ResourcesTokenController.Amount, Is.EqualTo(0));

        }

        [UnityTest]
        public IEnumerator Move_Resource_To_Pile()
        {
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(_investigatorsProvider.First.InvestigatorCard, _investigatorsProvider.First.InvestigatorZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<GainResourceGameAction>().SetWith(_investigatorsProvider.First, 5).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<PayResourceGameAction>().SetWith(_investigatorsProvider.First, 2).Execute().AsCoroutine();

            Assert.That(_areaInvestigatorViewsManager.Get(_investigatorsProvider.First).ResourcesTokenController.Amount, Is.EqualTo(3));
        }

        [UnityTest]
        public IEnumerator Move_Resource_Swaping()
        {
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(_investigatorsProvider.First.InvestigatorCard, _investigatorsProvider.First.InvestigatorZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(_investigatorsProvider.Second.InvestigatorCard, _investigatorsProvider.Second.InvestigatorZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(_investigatorsProvider.First.FullDeck.First(), _investigatorsProvider.First.AidZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(_investigatorsProvider.Second.FullDeck.First(), _investigatorsProvider.Second.AidZone).Execute().AsCoroutine();

            yield return _gameActionsProvider.Create<GainResourceGameAction>().SetWith(_investigatorsProvider.First, 5).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<GainResourceGameAction>().SetWith(_investigatorsProvider.Second, 5).Execute().AsCoroutine();

            Assert.That(_areaInvestigatorViewsManager.Get(_investigatorsProvider.First).ResourcesTokenController.Amount, Is.EqualTo(5));
            Assert.That(_areaInvestigatorViewsManager.Get(_investigatorsProvider.Second).ResourcesTokenController.Amount, Is.EqualTo(5));
        }
    }
}
