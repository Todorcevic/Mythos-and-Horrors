using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class FinishChallengeGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly IPresenter<FinishChallengeGameAction> _finishChallengePresenter;

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new DiscardCommitsCards());
            await _gameActionsProvider.Create(new RestoreChallengeTokens());
            await _finishChallengePresenter.PlayAnimationWith(this);
        }
    }
}
