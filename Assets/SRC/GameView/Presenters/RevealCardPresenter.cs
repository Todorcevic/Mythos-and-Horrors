using System.Threading.Tasks;
using DG.Tweening;
using MythsAndHorrors.GameRules;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class RevealCardPresenter : IPresenter
    {
        [Inject] private readonly CardViewsManager _cardViewsManager;

        /*******************************************************************/
        async Task IPresenter.CheckGameAction(GameAction gameAction)
        {
            if (gameAction is RevealPlaceGameAction revealCard)
                await RevealCardWith(revealCard.Card);
        }

        /*******************************************************************/
        private async Task RevealCardWith(CardPlace cardPlace)
        {
            CardView cardToReveal = _cardViewsManager.GetCardView(cardPlace);

            if (cardToReveal is PlaceCardView placeCardView)
            {
                await placeCardView.RevealAnimation().AsyncWaitForCompletion();
            }
        }
    }
}
