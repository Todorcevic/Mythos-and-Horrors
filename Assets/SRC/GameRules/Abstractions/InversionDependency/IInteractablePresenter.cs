using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public interface IInteractablePresenter
    {
        Task<Effect> SelectWith(GameAction gamAction);
    }
}
