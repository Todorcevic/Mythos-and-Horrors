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
        [Inject] private readonly CardViewsManager _cardViewsManager;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Move_Resource_From_Card()
        {
            _prepareGameUse.Execute();
            CardSupply cardSupply = _investigatorsProvider.Leader.Cards[0] as CardSupply;

            yield return _gameActionFactory.Create<MoveCardsGameAction>().Run(cardSupply, _chaptersProvider.CurrentScene.PlotZone).AsCoroutine();
            yield return _gameActionFactory.Create(new UpdateStatGameAction(cardSupply.Cost, 8)).AsCoroutine();
            while (DEBUG_MODE)
            {
                yield return _gameActionFactory.Create(new UpdateStatGameAction(cardSupply.Cost, 1)).AsCoroutine();
                yield return PressAnyKey();
                yield return _gameActionFactory.Create(new UpdateStatGameAction(cardSupply.Cost, 8)).AsCoroutine();
                yield return PressAnyKey();
            }

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(cardSupply.Cost.Value, Is.EqualTo(8));
            Assert.That((_cardViewsManager.Get(cardSupply) as DeckCardView).GetPrivateMember<StatView>("_cost").Stat.Value, Is.EqualTo(8));
        }
    }
}
