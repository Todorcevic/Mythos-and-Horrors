using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class CardsDescriptionPresenter
    {
        [Inject] private readonly CardViewsManager _cardViewsManager;

        /*******************************************************************/
        public async Task RefreshDescriptions()
        {
            _cardViewsManager.AllCardsView.ForEach(cardView => cardView.RefreshDescription());
            await Task.CompletedTask;
        }
    }
}
