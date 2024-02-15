using System.Threading.Tasks;
using DG.Tweening;
using MythsAndHorrors.GameRules;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class RevealCardPresenter : IPresenter
    {
        [Inject] private readonly MoveCardHandler _moveCardHandler;
        [Inject] private readonly CardViewsManager _cardViewsManager;

        /*******************************************************************/
        async Task IPresenter.CheckGameAction(GameAction gameAction)
        {
            if (gameAction is RevealCardGameAction revealCard)
                await RevealCardWith(revealCard);
        }

        /*******************************************************************/
        private async Task RevealCardWith(RevealCardGameAction revealCard)
        {
            CardView cardToReveal = _cardViewsManager.GetCardView(revealCard.Card);

            if (cardToReveal is PlaceCardView placeCardView)
                await RevealAnimation(placeCardView);
        }

        private async Task RevealAnimation(PlaceCardView placeCardView)
        {
            await _moveCardHandler.MoveCardToCenter(placeCardView);
            await placeCardView.RevealAnimation().AsyncWaitForCompletion();
            await _moveCardHandler.MoveCardWithPreviewToZone(placeCardView.Card, placeCardView.Card.CurrentZone);
        }
    }
}
