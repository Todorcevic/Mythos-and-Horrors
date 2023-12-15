using System;
using System.Collections.Generic;
using System.Linq;

namespace MythsAndHorrors.GameRules
{
    public class GameStateService
    {
        public GameAction CurrentAction { get; private set; }

        /*******************************************************************/
        public void SetCurrentAction(GameAction gameAction)
        {
            CurrentAction = gameAction ?? throw new ArgumentNullException(nameof(gameAction) + " gameAction cant be null");
        }
    }
}
