﻿using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    [TestFixture]
    public class ResourceGameActionTests : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUse;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly AreaInvestigatorViewsManager _areaInvestigatorViewsManager;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Move_Resource_From_Card()
        {
            _prepareGameUse.Execute();
            CardSupply cardSupply = _investigatorsProvider.Leader.Cards[0] as CardSupply;

            yield return _gameActionFactory.Create(new MoveCardsGameAction(cardSupply, _investigatorsProvider.Leader.AidZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new GainResourceGameAction(_investigatorsProvider.Leader, cardSupply.ResourceCost, 2)).AsCoroutine();

            Assert.That(_areaInvestigatorViewsManager.Get(_investigatorsProvider.Leader).ResourcesTokenController.Amount, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator Move_Resource_To_Card()
        {
            _prepareGameUse.Execute();
            CardSupply cardSupply = _investigatorsProvider.Leader.Cards[0] as CardSupply;

            yield return _gameActionFactory.Create(new MoveCardsGameAction(cardSupply, _investigatorsProvider.Leader.AidZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new GainResourceGameAction(_investigatorsProvider.Leader, cardSupply.ResourceCost, 5)).AsCoroutine();
            yield return _gameActionFactory.Create(new PayResourceGameAction(_investigatorsProvider.Leader, cardSupply.ResourceCost, 5)).AsCoroutine();

            Assert.That(_areaInvestigatorViewsManager.Get(_investigatorsProvider.Leader).ResourcesTokenController.Amount, Is.EqualTo(0));
        }

        [UnityTest]
        public IEnumerator Move_Resource_To_Investigator()
        {
            _prepareGameUse.Execute();

            do
            {
                yield return _gameActionFactory.Create(new GainResourceGameAction(_investigatorsProvider.Leader, _chaptersProvider.CurrentScene.PileAmount, 5)).AsCoroutine();
                if (DEBUG_MODE) yield return PressAnyKey();
            } while (DEBUG_MODE);

            Assert.That(_areaInvestigatorViewsManager.Get(_investigatorsProvider.Leader).ResourcesTokenController.Amount, Is.EqualTo(5));
        }

        [UnityTest]
        public IEnumerator Move_Resource_To_Pile()
        {
            _prepareGameUse.Execute();

            do
            {
                yield return _gameActionFactory.Create(new GainResourceGameAction(_investigatorsProvider.Leader, _chaptersProvider.CurrentScene.PileAmount, 5)).AsCoroutine();
                yield return _gameActionFactory.Create(new PayResourceGameAction(_investigatorsProvider.Leader, _chaptersProvider.CurrentScene.PileAmount, 5)).AsCoroutine();

                if (DEBUG_MODE) yield return PressAnyKey();
            } while (DEBUG_MODE);

            Assert.That(_areaInvestigatorViewsManager.Get(_investigatorsProvider.Leader).ResourcesTokenController.Amount, Is.EqualTo(0));
        }

        [UnityTest]
        public IEnumerator Move_Resource_Swaping()
        {
            _prepareGameUse.Execute();

            yield return _gameActionFactory.Create(new MoveCardsGameAction(_investigatorsProvider.Leader.Cards[0], _investigatorsProvider.Leader.AidZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(_investigatorsProvider.Second.Cards[0], _investigatorsProvider.Second.AidZone)).AsCoroutine();

            yield return _gameActionFactory.Create(new GainResourceGameAction(_investigatorsProvider.Leader, _chaptersProvider.CurrentScene.PileAmount, 5)).AsCoroutine();
            yield return _gameActionFactory.Create(new GainResourceGameAction(_investigatorsProvider.Second, _chaptersProvider.CurrentScene.PileAmount, 5)).AsCoroutine();

            Assert.That(_areaInvestigatorViewsManager.Get(_investigatorsProvider.Second).ResourcesTokenController.Amount, Is.EqualTo(5));
        }
    }
}