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
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.First.InvestigatorCard, _investigatorsProvider.First.InvestigatorZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new GainResourceGameAction(_investigatorsProvider.First, 5)).AsCoroutine();
            yield return _gameActionsProvider.Create(new PayResourceGameAction(_investigatorsProvider.First, 2)).AsCoroutine();

            Assert.That(_areaInvestigatorViewsManager.Get(_investigatorsProvider.First).ResourcesTokenController.Amount, Is.EqualTo(3));
        }

        [UnityTest]
        public IEnumerator Move_Resource_Swaping()
        {
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.First.InvestigatorCard, _investigatorsProvider.First.InvestigatorZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.Second.InvestigatorCard, _investigatorsProvider.Second.InvestigatorZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.First.FullDeck.First(), _investigatorsProvider.First.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.Second.FullDeck.First(), _investigatorsProvider.Second.AidZone)).AsCoroutine();

            yield return _gameActionsProvider.Create(new GainResourceGameAction(_investigatorsProvider.First, 5)).AsCoroutine();
            yield return _gameActionsProvider.Create(new GainResourceGameAction(_investigatorsProvider.Second, 5)).AsCoroutine();

            Assert.That(_areaInvestigatorViewsManager.Get(_investigatorsProvider.First).ResourcesTokenController.Amount, Is.EqualTo(5));
            Assert.That(_areaInvestigatorViewsManager.Get(_investigatorsProvider.Second).ResourcesTokenController.Amount, Is.EqualTo(5));
        }
    }
}
