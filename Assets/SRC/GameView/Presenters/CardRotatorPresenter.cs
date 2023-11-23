using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.Utilities;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class CardRotatorPresenter
    {
        [Inject] private readonly CardsViewsManager _cardsManager;

        /*******************************************************************/
        public async Task TurnCards(float timeAnimation = ViewValues.FAST_TIME_ANIMATION, params Card[] cards)
        {
            Sequence sequence = DOTween.Sequence();
            cards.ForEach(card => sequence.Append(_cardsManager.Get(card).Rotate(timeAnimation)));
            await sequence.Play().AsyncWaitForCompletion();
        }

        public async Task TurnTogetherCards(float timeAnimation = ViewValues.FAST_TIME_ANIMATION, params Card[] cards)
        {
            Sequence sequence = DOTween.Sequence();
            cards.ForEach(card => sequence.Join(_cardsManager.Get(card).Rotate(timeAnimation)));
            await sequence.Play().AsyncWaitForCompletion();
        }
    }
}
