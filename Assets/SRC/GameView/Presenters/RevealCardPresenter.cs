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
            if (gameAction is RevealGameAction revealCard) await RevealCardPlaceWith(revealCard.Card);
        }

        /*******************************************************************/
        private async Task RevealCardPlaceWith(Card card)
        {
            await _cardViewsManager.GetCardView(card).RevealAnimation().AsyncWaitForCompletion();
        }
    }
}
