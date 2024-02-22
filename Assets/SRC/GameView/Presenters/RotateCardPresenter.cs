using Zenject;
using System.Threading.Tasks;
using MythsAndHorrors.GameRules;
using DG.Tweening;

namespace MythsAndHorrors.GameView
{
    public class RotateCardPresenter : INewPresenter<RotateCardGameAction>
    {
        [Inject] private readonly CardViewsManager _cardsManager;
        [Inject] private readonly MoveCardHandler _moveCardHandler;

        /*******************************************************************/
        async Task INewPresenter<RotateCardGameAction>.PlayAnimationWith(RotateCardGameAction rotateCardGameAction)
        {
            await RotateCard(rotateCardGameAction);
        }

        /*******************************************************************/
        private async Task RotateCard(RotateCardGameAction turnCardGameAction)
        {
            CardView cardView = _cardsManager.GetCardView(turnCardGameAction.Card);
            await _moveCardHandler.MoveCardToCenter(cardView).Join(cardView.Rotate().AsyncWaitForCompletion());
        }
    }
}
