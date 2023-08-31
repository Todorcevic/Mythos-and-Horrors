using System.Threading.Tasks;
using Zenject;

namespace GameRules
{
    public class WaitingForSelectionGameAction : GameAction
    {
        [Inject] private readonly CardRepository _cardRepository;

        private static Card cardSelected;
        private static TaskCompletionSource<bool> waitForSelection;

        /*******************************************************************/
        public async Task<Card> Run()
        {
            await Start();
            return cardSelected;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            waitForSelection = new();
            await waitForSelection.Task;
        }

        public static void Clicked(Card card)
        {
            cardSelected = card;
            waitForSelection.SetResult(true);
        }
    }
}
