using Zenject;
using System.Threading.Tasks;
using MythsAndHorrors.GameRules;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;

namespace MythsAndHorrors.GameView
{
    public class TokenMoverPresenter : IPresenter
    {
        [Inject(Id = ViewValues.CENTER_SHOW_POSITION)] private readonly Transform _centerShowPosition;
        [Inject] private readonly AreaInvestigatorViewsManager _areaInvestigatorViewsManager;
        [Inject] private readonly TokensGeneratorComponent _tokensGeneratorComponent;
        [Inject] private readonly SwapInvestigatorPresenter _swapInvestigatorPresenter;
        [Inject] private readonly StatableManager _statsViewsManager;

        /*******************************************************************/
        public async Task GainHints(GainHintGameAction gainHintGameAction)
        {
            TokenController hintsTokenController = _areaInvestigatorViewsManager.Get(gainHintGameAction.Investigator).HintsTokenController;
            List<TokenView> allTokens = _tokensGeneratorComponent.GetHintTokens(gainHintGameAction.Amount);
            Transform origin = _statsViewsManager.Get(gainHintGameAction.FromStat).StatTransform;
            await _swapInvestigatorPresenter.Select(gainHintGameAction.Investigator);
            await Gain(allTokens, origin, hintsTokenController);
        }

        public async Task PayHints(PayHintGameAction payHintGameAction)
        {
            TokenController hintsTokenController = _areaInvestigatorViewsManager.Get(payHintGameAction.Investigator).HintsTokenController;
            List<TokenView> allTokens = _tokensGeneratorComponent.GetHintTokens(payHintGameAction.Amount);
            Transform destiny = _statsViewsManager.Get(payHintGameAction.ToStat).StatTransform;
            await _swapInvestigatorPresenter.Select(payHintGameAction.Investigator);
            await Pay(allTokens, destiny, hintsTokenController);
        }

        public async Task GainResource(GainResourceGameAction gainResourceGameAction)
        {
            TokenController resourcesTokenController = _areaInvestigatorViewsManager.Get(gainResourceGameAction.Investigator).ResourcesTokenController;
            List<TokenView> allTokens = _tokensGeneratorComponent.GetResourceTokens(gainResourceGameAction.Amount);
            Transform origin = _statsViewsManager.Get(gainResourceGameAction.FromStat).StatTransform;
            await _swapInvestigatorPresenter.Select(gainResourceGameAction.Investigator);
            await Gain(allTokens, origin, resourcesTokenController);
        }

        public async Task PayResource(PayResourceGameAction payResourceGameAction)
        {
            TokenController resourcesTokenController = _areaInvestigatorViewsManager.Get(payResourceGameAction.Investigator).ResourcesTokenController;
            List<TokenView> allTokens = _tokensGeneratorComponent.GetResourceTokens(payResourceGameAction.Amount);
            Transform destiny = _statsViewsManager.Get(payResourceGameAction.ToStat).StatTransform;
            await _swapInvestigatorPresenter.Select(payResourceGameAction.Investigator);
            await Pay(allTokens, destiny, resourcesTokenController);
        }

        private async Task Gain(List<TokenView> allTokens, Transform origin, TokenController tokenController)
        {
            await MoveUp(allTokens, origin, null);
            await MoveDown(allTokens, tokenController.TokenOff.transform, () => tokenController.TokenOff.SetAmount(1));
        }

        private async Task Pay(List<TokenView> allTokens, Transform destiny, TokenController tokenController)
        {
            await MoveUp(allTokens, tokenController.TokenOn.transform, () => tokenController.TokenOn.SetAmount(0));
            await MoveDown(allTokens, destiny, null);
        }

        private async Task MoveUp(List<TokenView> allTokens, Transform startPosition, TweenCallback starting)
        {
            Sequence sequence = DOTween.Sequence();
            foreach (TokenView tokenView in allTokens)
            {
                Vector3 randomPosition = (Random.insideUnitSphere * 5f) + Vector3.up * Random.Range(-0.2f, 0.2f);
                sequence.Join(DOTween.Sequence()
                .PrependCallback(() => tokenView.transform.position = startPosition.position)
                .PrependCallback(() => tokenView.SetAmount(1))
                .PrependCallback(starting)
                .Append(tokenView.transform.DOMove(_centerShowPosition.position + randomPosition, ViewValues.FAST_TIME_ANIMATION * Random.Range(1.5f, 2.5f)).SetEase(Ease.OutCubic))
                .Join(tokenView.transform.DOScale(2f, ViewValues.FAST_TIME_ANIMATION))
                .Join(tokenView.transform.DORotate(_centerShowPosition.rotation.eulerAngles + new Vector3(180, 0, 0), ViewValues.FAST_TIME_ANIMATION)));
            }

            await sequence.AsyncWaitForCompletion();
        }

        private async Task MoveDown(List<TokenView> allTokens, Transform target, TweenCallback finish)
        {
            Sequence sequence = DOTween.Sequence();
            foreach (TokenView tokenView in allTokens)
            {
                sequence.Join(DOTween.Sequence()
                .Append(tokenView.transform.DOMove(target.position, ViewValues.FAST_TIME_ANIMATION * Random.Range(1.5f, 2.5f)).SetEase(Ease.InCubic))
                .Join(tokenView.transform.DOScale(1f, ViewValues.FAST_TIME_ANIMATION))
                .Join(tokenView.transform.DORotate(target.rotation.eulerAngles + new Vector3(0, 0, 180), ViewValues.FAST_TIME_ANIMATION))
                .AppendCallback(() => tokenView.SetAmount(0))
                .AppendCallback(finish));
            }

            await sequence.AsyncWaitForCompletion();
        }
    }
}
