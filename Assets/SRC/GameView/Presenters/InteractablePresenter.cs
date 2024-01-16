using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class InteractablePresenter : IInteractableAnimator
    {
        [Inject] private readonly ActivatorUIPresenter _activatorUIPresenter;

        private TaskCompletionSource<bool> waitForSelection;
        private Card cardSelected;

        /*******************************************************************/
        public async Task<Card> Interact(List<Card> activablesCards)
        {
            waitForSelection = new();
            _activatorUIPresenter.ActivateAll(activablesCards);
            await waitForSelection.Task;
            _activatorUIPresenter.DeactivateAll();
            await Task.Delay(250);
            return cardSelected;
        }

        public void Clicked(Card card = null)
        {
            cardSelected = card;
            waitForSelection.SetResult(true);
        }
    }
}
