using System.Threading.Tasks;
using DG.Tweening;
using MythosAndHorrors.GameRules;
using Zenject;

namespace MythosAndHorrors.GameView
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

        /*******************************************************************/
        Task IPresenter<RevealGameAction>.UndoAnimationWith(RevealGameAction gameAction)
        {
            throw new System.NotImplementedException();
        }
    }
}
