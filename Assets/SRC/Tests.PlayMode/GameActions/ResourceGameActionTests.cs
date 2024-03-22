using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    [TestFixture]
    public class ResourceGameActionTests : TestBase
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly AreaInvestigatorViewsManager _areaInvestigatorViewsManager;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Full_Take_Resource()
        {
            yield return PlayThisInvestigator(_investigatorsProvider.First);
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(_investigatorsProvider.First.CurrentTurns, GameValues.DEFAULT_TURNS_AMOUNT)).AsCoroutine();

            if (!DEBUG_MODE) WaitToTokenClick().AsTask();
            yield return _gameActionsProvider.Create(new OneInvestigatorTurnGameAction(_investigatorsProvider.First)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(_areaInvestigatorViewsManager.Get(_investigatorsProvider.First).ResourcesTokenController.Amount, Is.EqualTo(1));
            Assert.That(_investigatorsProvider.First.Resources.Value, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator Move_Resource_To_Investigator()
        {
            yield return PlayThisInvestigator(_investigatorsProvider.First);
            do
            {
                yield return _gameActionsProvider.Create(new GainResourceGameAction(_investigatorsProvider.First, 5)).AsCoroutine();
                if (DEBUG_MODE) yield return PressAnyKey();
            } while (DEBUG_MODE);

            Assert.That(_areaInvestigatorViewsManager.Get(_investigatorsProvider.First).ResourcesTokenController.Amount, Is.EqualTo(5));
        }

        [UnityTest]
        public IEnumerator Move_Resource_To_Pile()
        {
            yield return PlayThisInvestigator(_investigatorsProvider.First);
            do
            {
                yield return _gameActionsProvider.Create(new GainResourceGameAction(_investigatorsProvider.First, 5)).AsCoroutine();
                if (DEBUG_MODE) yield return PressAnyKey();
                yield return _gameActionsProvider.Create(new PayResourceGameAction(_investigatorsProvider.First, 2)).AsCoroutine();

                if (DEBUG_MODE) yield return PressAnyKey();
            } while (DEBUG_MODE);

            Assert.That(_areaInvestigatorViewsManager.Get(_investigatorsProvider.First).ResourcesTokenController.Amount, Is.EqualTo(3));
        }

        [UnityTest]
        public IEnumerator Move_Resource_Swaping()
        {
            yield return PlayThisInvestigator(_investigatorsProvider.First);
            yield return PlayThisInvestigator(_investigatorsProvider.Second);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.First.Cards[0], _investigatorsProvider.First.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_investigatorsProvider.Second.Cards[0], _investigatorsProvider.Second.AidZone)).AsCoroutine();

            yield return _gameActionsProvider.Create(new GainResourceGameAction(_investigatorsProvider.First, 5)).AsCoroutine();
            yield return _gameActionsProvider.Create(new GainResourceGameAction(_investigatorsProvider.Second, 5)).AsCoroutine();

            Assert.That(_areaInvestigatorViewsManager.Get(_investigatorsProvider.Second).ResourcesTokenController.Amount, Is.EqualTo(5));
        }
    }
}
