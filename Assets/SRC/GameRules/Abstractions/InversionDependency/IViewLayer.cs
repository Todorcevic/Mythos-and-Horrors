using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IViewLayer
    {
        Task PlayAnimationWith(GameAction gameAction);
        Task<Card> StartSelectionWith(InteractableGameAction interactableGameAction);
    }
}
