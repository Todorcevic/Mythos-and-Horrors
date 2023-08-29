using System.Threading.Tasks;
using Zenject;

namespace GameRules
{
    public class WaitingForSelectionGameAction : GameAction
    {
        [Inject] private readonly CardRepository _cardRepository;

        private static string cardSelected;
        private static TaskCompletionSource<bool> waitForSelection;

        /*******************************************************************/
        public async Task<Card> Run()
        {
            await Start();
            return _cardRepository.GetCard(cardSelected);
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            waitForSelection = new();
            await waitForSelection.Task;
        }

        public static void Clicked(string cardId)
        {
            cardSelected = cardId;
            waitForSelection.SetResult(true);
        }
    }
}
