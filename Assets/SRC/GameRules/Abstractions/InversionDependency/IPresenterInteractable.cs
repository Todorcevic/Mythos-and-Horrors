using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public interface IPresenterInteractable
    {
        Task<BaseEffect> SelectWith(InteractableGameAction interactableGameAction);
    }
}
