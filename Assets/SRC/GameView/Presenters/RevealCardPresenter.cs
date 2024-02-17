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
            if (gameAction is RevealGameAction revealCard) await RevealCardPlaceWith(revealCard);
        }

        /*******************************************************************/
        private async Task RevealCardPlaceWith(RevealGameAction revealGameAction)
        {
            await _cardViewsManager.GetCardView(revealGameAction.Card).RevealAnimation().AsyncWaitForCompletion();
        }
    }
}
