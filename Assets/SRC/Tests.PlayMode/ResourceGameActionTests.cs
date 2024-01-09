using MythsAndHorrors.GameRules;
using MythsAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using Zenject;

namespace MythsAndHorrors.PlayMode.Tests
{
    [TestFixture]
    public class ResourceGameActionTests : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUse;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly AreaInvestigatorViewsManager _areaInvestigatorViewsManager;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Move_Resource_To_Investigator()
        {
            // DEBUG_MODE = true;
            _prepareGameUse.Execute();

            do
            {
                yield return _gameActionFactory.Create<GainResourceGameAction>()
                    .Run(_investigatorsProvider.Leader, _cardsProvider.Resource, 5).AsCoroutine();
                if (DEBUG_MODE) yield return PressAnyKey();
            } while (DEBUG_MODE);

            Assert.That(_areaInvestigatorViewsManager.Get(_investigatorsProvider.Leader).ResourcesTokenController.Amount, Is.EqualTo(5));
        }

        [UnityTest]
        public IEnumerator Move_Resource_To_Pile()
        {
            // DEBUG_MODE = true;
            _prepareGameUse.Execute();
            yield return _gameActionFactory.Create<GainResourceGameAction>()
                .Run(_investigatorsProvider.Leader, _cardsProvider.Resource, 5).AsCoroutine();

            do
            {
                yield return _gameActionFactory.Create<PayResourceGameAction>()
                .Run(_investigatorsProvider.Leader, _cardsProvider.Resource, 5).AsCoroutine();

                if (DEBUG_MODE) yield return PressAnyKey();
            } while (DEBUG_MODE);

            Assert.That(_areaInvestigatorViewsManager.Get(_investigatorsProvider.Leader).ResourcesTokenController.Amount, Is.EqualTo(0));
        }

        [UnityTest]
        public IEnumerator Move_Resource_Swaping()
        {
            // DEBUG_MODE = true;
            _prepareGameUse.Execute();
            yield return _gameActionFactory.Create<GainResourceGameAction>()
                .Run(_investigatorsProvider.Leader, _cardsProvider.Resource, 5).AsCoroutine();

            yield return _gameActionFactory.Create<GainResourceGameAction>()
                .Run(_investigatorsProvider.Second, _cardsProvider.Resource, 5).AsCoroutine();


            Assert.That(_areaInvestigatorViewsManager.Get(_investigatorsProvider.Second).ResourcesTokenController.Amount, Is.EqualTo(5));
        }
    }
}
