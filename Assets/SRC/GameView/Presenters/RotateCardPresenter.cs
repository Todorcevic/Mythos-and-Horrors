using Zenject;
using System.Threading.Tasks;
using MythsAndHorrors.GameRules;
using DG.Tweening;

namespace MythsAndHorrors.GameView
{
    public class RotateCardPresenter : IPresenter
    {
        [Inject] private readonly CardViewsManager _cardsManager;
        [Inject] private readonly MoveCardHandler _moveCardHandler;

        /*******************************************************************/
        async Task IPresenter.CheckGameAction(GameAction gamAction)
        {
            if (gamAction is RotateCardGameAction rotateCardGameAction)
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
