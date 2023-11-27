using DG.Tweening;
using MythsAndHorrors.EditMode;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.PlayMode
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
