using DG.Tweening;
using MythsAndHorrors.GameRules;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class CardRotatorPresenter : IAnimatorEnd
    {
        [Inject] private readonly CardViewsManager _cardsManager;

        /*******************************************************************/
        public async Task CheckingAtEnd(GameAction gameAction)
        {
            if (gameAction is RotateCardGameAction rotateCardGameAction)
                await Rotate(rotateCardGameAction.Card);
        }

        /*******************************************************************/
        public async Task Rotate(Card card)
        {
            await _cardsManager.Get(card).Rotate().AsyncWaitForCompletion();
        }
    }
}
