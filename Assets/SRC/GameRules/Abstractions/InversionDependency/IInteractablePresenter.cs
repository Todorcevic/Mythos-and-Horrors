using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public interface IInteractablePresenter
    {
        Task<BaseEffect> SelectWith(InteractableGameAction interactableGameAction);
    }
}
