using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IInteractablePresenter
    {
        Task<Effect> SelectWith(GameAction gamAction);
    }
}
