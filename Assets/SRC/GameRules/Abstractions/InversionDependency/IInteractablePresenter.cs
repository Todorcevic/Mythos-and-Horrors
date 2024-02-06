using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IInteractablePresenter
    {
        Task<Effect> CheckGameAction(GameAction gamAction);
    }
}
