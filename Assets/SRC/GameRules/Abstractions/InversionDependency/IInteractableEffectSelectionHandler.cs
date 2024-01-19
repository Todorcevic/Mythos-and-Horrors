using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IInteractableEffectSelectionHandler
    {
        Task<Effect> Interact(MultiEffectSelectionGameAction multiEffectSelectionGameAction);
    }
}
