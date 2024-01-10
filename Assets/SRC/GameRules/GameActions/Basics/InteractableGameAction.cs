using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public abstract class InteractableGameAction : GameAction
    {
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
            await waitForSelection.Task;
        }

        public void Continue(Card card = null)
        {
            cardSelected = card;
            waitForSelection.SetResult(true);
        }
    }
}
