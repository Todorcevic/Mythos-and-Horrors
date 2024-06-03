using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public interface IInteractablePresenter
    {
        Task<BaseEffect> SelectWith(GameAction gamAction);
    }
}
