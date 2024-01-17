using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public abstract class InteractableGameAction : GameAction
    {
        [Inject] private readonly IInteractableAnimator _interactableAnimator;

        public abstract List<Card> ActivableCards { get; }
        public Card CardSelected { get; private set; }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            CardSelected = await _interactableAnimator.Interact(this);
        }
    }
}
