using DG.Tweening;
using MythosAndHorrors.GameRules;
using System.Threading.Tasks;
using UnityEngine;
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
                AudioClip audio = _audioComponent.GetAudioEffect(cardEffect);
                if (audio == null) return;
                Tween showAnimation = DOTween.Sequence().Join(_moveCardHandler.MoveCardtoCenter(cardEffect.CardOwner))
                     .Append(_cardViewsManager.GetCardView(cardEffect.CardOwner).ShowEffect());
                await _audioComponent.PlayAudioAsync(audio);
                showAnimation.Kill();
                _moveCardHandler.ReturnCard(cardEffect.CardOwner);
            }
        }
    }
}
