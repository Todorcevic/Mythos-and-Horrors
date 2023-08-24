using GameRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameView
{
    public class GameActionSelectorPresenter : IGameActionSelecter
    {
        public async Task ShowThisActions(params GameAction[] gameActions)
        {
            await Task.CompletedTask;
        }
    }
}
