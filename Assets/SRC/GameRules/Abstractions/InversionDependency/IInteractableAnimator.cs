using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IInteractableAnimator
    {
        Task<Card> Interact(InteractableGameAction interactableGameAction);
    }
}
