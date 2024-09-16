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
            AvatarView avatar = _avatarViewsManager.Get(eliminateInvestigatorGameAction.Investigator);
            avatar.gameObject.SetActive(!avatar.gameObject.activeSelf);
            await Task.CompletedTask;
        }
    }
}
