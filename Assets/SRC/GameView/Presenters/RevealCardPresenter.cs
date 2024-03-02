using System.Threading.Tasks;
using DG.Tweening;
using MythsAndHorrors.GameRules;
using Zenject;

namespace MythsAndHorrors.GameView
{

    public class RevealCardPresenter : IPresenter<RevealGameAction>
    {
        [Inject] private readonly CardViewsManager _cardViewsManager;

        /*******************************************************************/
        async Task IPresenter<RevealGameAction>.PlayAnimationWith(RevealGameAction revealCard)
        {
            await RevealCardPlaceWith(revealCard);
        }

        /*******************************************************************/
        private async Task RevealCardPlaceWith(RevealGameAction revealGameAction)
        {
            await _cardViewsManager.GetCardView(revealGameAction.Card).RevealAnimation().AsyncWaitForCompletion();
        }
    }
}
