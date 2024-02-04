using Zenject;
using System.Threading.Tasks;
using MythsAndHorrors.GameRules;
using DG.Tweening;

namespace MythsAndHorrors.GameView
{
    public class TurnCardPresenter : IPresenter
    {
        [Inject] private readonly CardViewsManager _cardsManager;
        [Inject] private readonly MoveCardHandler _moveCardHandler;

        /*******************************************************************/
        public async Task TurnCard(TurnCardGameAction turnCardGameAction)
        {
            CardView cardView = _cardsManager.GetCardView(turnCardGameAction.Card);
            await _moveCardHandler.MoveCardToCenter(cardView).Join(cardView.Rotate().AsyncWaitForCompletion());
        }
    }
}
