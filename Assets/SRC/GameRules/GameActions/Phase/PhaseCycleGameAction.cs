using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PhaseCycleGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new InvestigatorsPhaseGameAction());
            await _gameActionsProvider.Create(new CreaturePhaseGameAction());
            await _gameActionsProvider.Create(new RestorePhaseGameAction());
            await _gameActionsProvider.Create(new ScenePhaseGameAction());
            await _gameActionsProvider.Create(new PhaseCycleGameAction());
        }
    }
}
