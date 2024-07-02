using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class RoundGameAction : GameAction
    {
        public static int Round { get; private set; }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Round++;
            if (Round != 1) await _gameActionsProvider.Create<ScenePhaseGameAction>().Start();
            await _gameActionsProvider.Create<InvestigatorsPhaseGameAction>().Start();
            await _gameActionsProvider.Create<CreaturePhaseGameAction>().Start();
            await _gameActionsProvider.Create<RestorePhaseGameAction>().Start();
        }

        public override async Task Undo()
        {
            Round--;
            await base.Undo();
            await Task.CompletedTask;
        }
    }
}
