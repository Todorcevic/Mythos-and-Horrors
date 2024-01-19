using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IInteractableHandler
    {
        Task<Card> Interact(InteractableGameAction interactableGameAction);
    }
}
