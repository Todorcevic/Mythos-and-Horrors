using DG.Tweening;
using MythosAndHorrors.GameRules;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class PlayEffectPresenter
    {
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly AudioComponent _audioComponent;
        [Inject] private readonly MoveCardHandler _moveCardHandler;

        /*******************************************************************/
        public async Task PlayAnimationWith(PlayEffectGameAction playEffectGameAction)
        {
            if (playEffectGameAction.Effect is CardEffect cardEffect && cardEffect.CardOwner != null)
            {
                CardView cardView = _cardViewsManager.GetCardView(cardEffect.CardOwner);
                await _moveCardHandler.MoveCardtoCenter(cardEffect.CardOwner).AsyncWaitForCompletion();
                Tween showAnimation = cardView.ShowAnimation();
                await _audioComponent.PlayCardEffect(cardEffect);
                showAnimation.Kill();
                _moveCardHandler.ReturnCard(cardEffect.CardOwner);
            }
        }
    }
}
