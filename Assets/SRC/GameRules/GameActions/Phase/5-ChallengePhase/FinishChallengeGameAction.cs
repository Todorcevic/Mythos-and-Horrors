using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class FinishChallengeGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new DiscardCommitsCards());
            await _gameActionsProvider.Create(new RestoreChallengeTokens());
        }
    }
}
