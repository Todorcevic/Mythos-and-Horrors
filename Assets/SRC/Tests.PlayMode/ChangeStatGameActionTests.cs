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
    public class ChangeStatGameActionTests : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUse;
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Move_Resource_From_Card()
        {
            _prepareGameUse.Execute();
            CardSupply cardSupply = _investigatorsProvider.Leader.Cards[0] as CardSupply;

            yield return _gameActionFactory.Create<MoveCardsGameAction>().Run(cardSupply, _chaptersProvider.CurrentScene.SelectorZone).AsCoroutine();
            yield return _gameActionFactory.Create<UpdateStatGameAction>().Run(cardSupply.Cost, 8).AsCoroutine();
            while (DEBUG_MODE)
            {
                yield return _gameActionFactory.Create<UpdateStatGameAction>().Run(cardSupply.Cost, 1).AsCoroutine();
                yield return PressAnyKey();
                yield return _gameActionFactory.Create<UpdateStatGameAction>().Run(cardSupply.Cost, 8).AsCoroutine();
                yield return PressAnyKey();
            }

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(cardSupply.Health.Value, Is.EqualTo(2));
        }
    }
}
