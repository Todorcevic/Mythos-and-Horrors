using DG.Tweening;
using MythosAndHorrors.GameRules;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{


    public class PlayCardPresenter : IPresenter<PlayEffectGameAction>
    {
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly AudioComponent _audioComponent;
        [Inject] private readonly AnimationsManager _animationsManager;
        [Inject] private readonly MoveCardHandler _moveCardHandler;

        /*******************************************************************/
        public async Task PlayAnimationWith(PlayEffectGameAction playEffectGameAction)
        {
            if (playEffectGameAction.Effect is CardEffect cardEffect)
            {
                CardView cardView = _cardViewsManager.GetCardView(cardEffect.CardOwner);
                AudioClip clip = _animationsManager.GetAnimation(cardEffect);
                if (clip == null) return;
                await _moveCardHandler.MoveCardtoCenter(cardEffect.CardOwner).AsyncWaitForCompletion();
                Tween showAnimation = cardView.ShowAnimation();
                await _audioComponent.PlayAudio(clip);
                showAnimation.Kill();
                _moveCardHandler.ReturnCard(cardEffect.CardOwner);
            }
        }
    }
}
