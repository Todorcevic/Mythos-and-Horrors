﻿using DG.Tweening;
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
    public class PositionTest : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly GameActionFactory _gameActionFactory;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator All_Zones_With_Cards()
        {
            _prepareGameUseCase.Execute();
            Investigator investigator1 = _investigatorsProvider.Leader;

            yield return _gameActionFactory.Create(new MoveCardsGameAction(investigator1.InvestigatorCard, investigator1.InvestigatorZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(investigator1.Cards[1], investigator1.DiscardZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(investigator1.Cards[2], investigator1.DeckZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(investigator1.Cards.GetRange(3, 5), investigator1.AidZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(investigator1.Cards.GetRange(9, 5), investigator1.DangerZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(investigator1.Cards.GetRange(15, 8), investigator1.HandZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(investigator1.Cards.GetRange(24, 4), investigator1.DiscardZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(_chaptersProvider.CurrentScene.Info.Cards[30], _chaptersProvider.CurrentScene.PlotZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(_chaptersProvider.CurrentScene.Info.Cards[31], _chaptersProvider.CurrentScene.GoalZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(_chaptersProvider.CurrentScene.Info.Cards[3], _chaptersProvider.CurrentScene.DangerDeckZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(_chaptersProvider.CurrentScene.Info.Cards.GetRange(24, 4), _chaptersProvider.CurrentScene.DangerDiscardZone)).AsCoroutine();

            int k = 0;
            for (int i = 0; i < _chaptersProvider.CurrentScene.PlaceZone.GetLength(0); i++)
            {
                for (int j = 0; j < _chaptersProvider.CurrentScene.PlaceZone.GetLength(1); j++)
                {
                    yield return _gameActionFactory.Create(new MoveCardsGameAction(_chaptersProvider.CurrentScene.Info.Cards[5 + k++], _chaptersProvider.CurrentScene.PlaceZone[i, j])).AsCoroutine();
                }
            }
            if (DEBUG_MODE) yield return _gameActionFactory.Create(new InteractableGameAction()).AsCoroutine();


            Assert.That(true);
        }
    }
}