﻿using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CheckHintsGoalGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (!_chaptersProvider.CurrentScene.CurrentGoal.IsComplete) return;
            await _gameActionsProvider.Create(new RevealGameAction(_chaptersProvider.CurrentScene.CurrentGoal));
        }
    }
}