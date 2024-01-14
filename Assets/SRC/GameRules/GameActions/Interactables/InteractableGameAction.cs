using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public abstract class InteractableGameAction : GameAction
    {
        [Inject] private readonly IUIActivable _UIActivable;
        private readonly TaskCompletionSource<bool> waitForSelection = new();
        private Card cardSelected;

        public abstract List<Card> ActivableCards { get; }

        /*******************************************************************/
        public async Task<Card> Run()
        {
            await Start();
            return cardSelected;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            _UIActivable.ActivateAll(ActivableCards);
            await waitForSelection.Task;
        }

        public void Continue(Card card = null)
        {
            cardSelected = card;
            waitForSelection.SetResult(true);
        }
    }
}
