using System;

namespace GameRules
{
    public class GameStateService : IGameStateEditable
    {
        public GameAction CurrentAction { get; private set; }

        /*******************************************************************/
        void IGameStateEditable.SetCurrentAction(GameAction gameAction)
        {
            CurrentAction = gameAction ?? throw new ArgumentNullException(nameof(gameAction) + " gameAction cant be null");
        }
    }
}
