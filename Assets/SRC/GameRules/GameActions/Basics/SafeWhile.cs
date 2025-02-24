using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class SafeWhile : GameAction
    {
        public Func<bool> Condition { get; private set; }
        public Func<Task> Logic { get; private set; }
        public State Initilized { get; private set; }

        /*******************************************************************/
        public SafeWhile SetWith(Func<bool> condition, Func<Task> logic)
        {
            Logic = logic;
            Condition = condition;
            Initilized = new State(false);
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Initilized, true).Execute();
            await ResolveLogic();
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Initilized, false).Execute();

            async Task ResolveLogic()
            {
                if (!Condition.Invoke() || !Initilized.IsActive) return;
                await Logic.Invoke();
                await ResolveLogic();
            }
        }
    }
}
