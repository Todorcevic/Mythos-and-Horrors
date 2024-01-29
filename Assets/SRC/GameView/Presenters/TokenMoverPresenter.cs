using Zenject;
using MythsAndHorrors.GameRules;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythsAndHorrors.GameView
{
    public class TokenMoverPresenter : IPresenter
    {
        [Inject] private readonly AreaInvestigatorViewsManager _areaInvestigatorViewsManager;
        [Inject] private readonly TokensGeneratorComponent _tokensGeneratorComponent;
        [Inject] private readonly SwapInvestigatorHandler _swapInvestigatorPresenter;
        [Inject] private readonly StatableManager _statableManager;
        [Inject] private readonly ZoneViewsManager _zonesManager;

        /*******************************************************************/
        public async Task GainHints(GainHintGameAction gainHintGameAction)
        {
            await _swapInvestigatorPresenter.Select(gainHintGameAction.Investigator).AsyncWaitForCompletion();
            await Gain().AsyncWaitForCompletion();

            Tween Gain()
            {
                Transform origin = _statableManager.Get(gainHintGameAction.FromStat).StatTransform;
                List<TokenView> allTokens = _tokensGeneratorComponent.GetHintTokens(gainHintGameAction.Amount);
                TokenController tokenController = _areaInvestigatorViewsManager.Get(gainHintGameAction.Investigator).HintsTokenController;

                return DOTween.Sequence()
                    .Append(MoveUp(allTokens, origin, null))
                    .Append(MoveDown(allTokens, tokenController.TokenOff.transform, () => tokenController.TokenOff.Activate()));
            }
        }

        public async Task PayHints(PayHintGameAction payHintGameAction)
        {
            await _swapInvestigatorPresenter.Select(payHintGameAction.Investigator).AsyncWaitForCompletion();
            await Pay().AsyncWaitForCompletion();

            Tween Pay()
            {
                TokenController hintsTokenController = _areaInvestigatorViewsManager.Get(payHintGameAction.Investigator).HintsTokenController;
                List<TokenView> allTokens = _tokensGeneratorComponent.GetHintTokens(payHintGameAction.Amount);
                Transform destiny = _statableManager.Get(payHintGameAction.ToStat).StatTransform;

                return DOTween.Sequence()
                    .Append(MoveUp(allTokens, hintsTokenController.TokenOn.transform, () => hintsTokenController.TokenOn.Deactivate()))
                    .Append(MoveDown(allTokens, destiny, null));
            }
        }

        public async Task GainResource(GainResourceGameAction gainResourceGameAction)
        {
            await _swapInvestigatorPresenter.Select(gainResourceGameAction.Investigator).AsyncWaitForCompletion();
            await Gain().AsyncWaitForCompletion();

            Tween Gain()
            {
                TokenController resourcesTokenController = _areaInvestigatorViewsManager.Get(gainResourceGameAction.Investigator).ResourcesTokenController;
                List<TokenView> allTokens = _tokensGeneratorComponent.GetResourceTokens(gainResourceGameAction.Amount);
                Transform origin = _statableManager.Get(gainResourceGameAction.FromStat).StatTransform;

                return DOTween.Sequence()
                   .Append(MoveUp(allTokens, origin, null))
                   .Append(MoveDown(allTokens, resourcesTokenController.TokenOff.transform, () => resourcesTokenController.TokenOff.Activate()));
            }
        }

        public async Task PayResource(PayResourceGameAction payResourceGameAction)
        {
            await _swapInvestigatorPresenter.Select(payResourceGameAction.Investigator).AsyncWaitForCompletion();
            await Pay().AsyncWaitForCompletion();

            Tween Pay()
            {
                TokenController resourcesTokenController = _areaInvestigatorViewsManager.Get(payResourceGameAction.Investigator).ResourcesTokenController;
                List<TokenView> allTokens = _tokensGeneratorComponent.GetResourceTokens(payResourceGameAction.Amount);
                Transform destiny = _statableManager.Get(payResourceGameAction.ToStat).StatTransform;

                return DOTween.Sequence()
                   .Append(MoveUp(allTokens, resourcesTokenController.TokenOn.transform, () => resourcesTokenController.TokenOn.Deactivate()))
                   .Append(MoveDown(allTokens, destiny, null));
            }
        }

        private Sequence MoveUp(List<TokenView> allTokens, Transform startPosition, TweenCallback starting)
        {
            Sequence sequence = DOTween.Sequence();
            foreach (TokenView tokenView in allTokens)
            {
                Vector3 randomPosition = (Random.insideUnitSphere * 5f) + Vector3.up * Random.Range(-0.2f, 0.2f);
                sequence.Join(DOTween.Sequence()
                .PrependCallback(() => tokenView.transform.position = startPosition.position)
                .PrependCallback(() => tokenView.Activate())
                .PrependCallback(starting)
                .Append(tokenView.transform.DOMove(_zonesManager.CenterShowZone.transform.position + randomPosition, ViewValues.FAST_TIME_ANIMATION * Random.Range(1.5f, 2.5f)).SetEase(Ease.OutCubic))
                .Join(tokenView.transform.DOScale(2f, ViewValues.FAST_TIME_ANIMATION))
                .Join(tokenView.transform.DORotate(_zonesManager.CenterShowZone.transform.rotation.eulerAngles, ViewValues.FAST_TIME_ANIMATION).SetEase(Ease.Linear)));
            }

            return sequence;
        }

        private Sequence MoveDown(List<TokenView> allTokens, Transform target, TweenCallback finish)
        {
            Sequence sequence = DOTween.Sequence();
            foreach (TokenView tokenView in allTokens)
            {
                sequence.Join(DOTween.Sequence()
                .Append(tokenView.transform.DOMove(target.position, ViewValues.FAST_TIME_ANIMATION * Random.Range(1.5f, 2.5f)).SetEase(Ease.InCubic))
                .Join(tokenView.transform.DOScale(1f, ViewValues.FAST_TIME_ANIMATION))
                .Join(tokenView.transform.DORotate(target.rotation.eulerAngles + new Vector3(0, 0, 180), ViewValues.FAST_TIME_ANIMATION).SetEase(Ease.Linear))
                .AppendCallback(() => tokenView.Deactivate())
                .AppendCallback(finish));
            }

            return sequence;
        }
    }
}
