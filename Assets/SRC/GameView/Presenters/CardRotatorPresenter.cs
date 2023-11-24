using DG.Tweening;
using MythsAndHorrors.GameRules;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class CardRotatorPresenter : ICardRotator
    {
        [Inject] private readonly CardViewsManager _cardsManager;

        /*******************************************************************/
        public void Rotate(Card card)
        {
            _cardsManager.Get(card).Rotate();
        }

        public async Task RotateAsync(Card card)
        {
            await _cardsManager.Get(card).Rotate().AsyncWaitForCompletion();
        }
    }
}
