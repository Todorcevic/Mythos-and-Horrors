using System;

namespace MythsAndHorrors.GameRules
{
    public class GameStateService
    {
        public Chapter CurrentChapter { get; set; }
        public Scene CurrentScene { get; set; }
        public GameAction CurrentAction { get; private set; }

        /*******************************************************************/
        public void SetCurrentAction(GameAction gameAction)
        {
            CurrentAction = gameAction ?? throw new ArgumentNullException(nameof(gameAction) + " gameAction cant be null");
        }
    }
}
