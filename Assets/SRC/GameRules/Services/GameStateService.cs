namespace GameRules
{
    public class GameStateService : IGameStateEditable
    {
        public GameAction CurrentAction { get; private set; }

        /*******************************************************************/
        void IGameStateEditable.SetCurrentAction(GameAction action) => CurrentAction = action;
    }
}
