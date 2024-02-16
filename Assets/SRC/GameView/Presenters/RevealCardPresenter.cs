using System.Threading.Tasks;
using DG.Tweening;
using MythsAndHorrors.GameRules;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class RevealCardPresenter : IPresenter
    {
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly ShowHistoryComponent _showHistoryComponent;

        /*******************************************************************/
        async Task IPresenter.CheckGameAction(GameAction gameAction)
        {
            if (gameAction is RevealGameAction revealCard && revealCard.RevellableCard is CardPlace cardPlace)
                await RevealCardPlaceWith(cardPlace);
        }

        /*******************************************************************/
        private async Task RevealCardPlaceWith(CardPlace cardPlace)
        {
            if (_cardViewsManager.GetCardView(cardPlace) is not PlaceCardView placeCardView) return;

            await placeCardView.RevealAnimation().AsyncWaitForCompletion();
            await _showHistoryComponent.Show(cardPlace.RevealHistory);
        }
    }
}
