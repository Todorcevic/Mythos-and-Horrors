using MythosAndHorrors.GameRules;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class EliminateInvestigatorPresenter
    {
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;

        /*******************************************************************/
        public async Task PlayAnimationWith(EliminateInvestigatorGameAction eliminateInvestigatorGameAction)
        {
            AvatarView avatar = _avatarViewsManager.Get(eliminateInvestigatorGameAction.Investigator);
            avatar.gameObject.SetActive(!avatar.gameObject.activeSelf);
            await Task.CompletedTask;
        }
    }
}
