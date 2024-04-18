using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class RoundGameAction : GameAction
    {
        public static int Round { get; private set; }
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Round++;
            if (Round != 1) await _gameActionsProvider.Create(new ScenePhaseGameAction());
            await _gameActionsProvider.Create(new InvestigatorsPhaseGameAction());
            await _gameActionsProvider.Create(new CreaturePhaseGameAction());
            await _gameActionsProvider.Create(new RestorePhaseGameAction());
        }
    }
}
