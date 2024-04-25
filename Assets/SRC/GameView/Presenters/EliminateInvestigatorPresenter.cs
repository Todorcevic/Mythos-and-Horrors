using MythosAndHorrors.GameRules;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class EliminateInvestigatorPresenter : IPresenter<EliminateInvestigatorGameAction>
    {
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;

        /*******************************************************************/
        async Task IPresenter<EliminateInvestigatorGameAction>.PlayAnimationWith(EliminateInvestigatorGameAction eliminateInvestigatorGameAction)
        {
            _avatarViewsManager.Get(eliminateInvestigatorGameAction.Investigator).gameObject.SetActive(false);
            await Task.CompletedTask;
        }
        /*******************************************************************/
    }
}
