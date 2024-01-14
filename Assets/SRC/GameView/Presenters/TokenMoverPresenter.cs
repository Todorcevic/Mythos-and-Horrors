using Zenject;
using System.Threading.Tasks;
using MythsAndHorrors.GameRules;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;

namespace MythsAndHorrors.GameView
{
    public class TokenMoverPresenter : IResourceAnimator, IHintAnimator
    {
        [Inject(Id = ViewValues.CENTER_SHOW_POSITION)] private readonly Transform _centerShowPosition;
        [Inject] private readonly AreaInvestigatorViewsManager _areaInvestigatorViewsManager;
        [Inject] private readonly TokensGeneratorComponent _tokensGeneratorComponent;
        [Inject] private readonly SwapInvestigatorPresenter _swapInvestigatorPresenter;
        [Inject] private readonly CardViewsManager _cardViewsManager;

        /*******************************************************************/
        public async Task GainHints(Investigator investigator, int amount, Card fromCard)
        {
            TokenController hintsTokenController = _areaInvestigatorViewsManager.Get(investigator).HintsTokenController;
            List<TokenView> allTokens = _tokensGeneratorComponent.GetHintTokens(amount);
            Transform origin = GetTransformFor(fromCard);
            await _swapInvestigatorPresenter.Select(investigator);
            await Gain(allTokens, origin, hintsTokenController);
        }

        public async Task PayHints(Investigator investigator, int amount, Card toCard)
        {
            TokenController hintsTokenController = _areaInvestigatorViewsManager.Get(investigator).HintsTokenController;
            List<TokenView> allTokens = _tokensGeneratorComponent.GetHintTokens(amount);
            Transform destiny = GetTransformFor(toCard);
            await _swapInvestigatorPresenter.Select(investigator);
            await Pay(allTokens, destiny, hintsTokenController);
        }

        public async Task GainResource(Investigator investigator, int amount, Card fromCard)
        {
            TokenController resourcesTokenController = _areaInvestigatorViewsManager.Get(investigator).ResourcesTokenController;
            List<TokenView> allTokens = _tokensGeneratorComponent.GetResourceTokens(amount);
            Transform origin = GetTransformFor(fromCard);
            await _swapInvestigatorPresenter.Select(investigator);
            await Gain(allTokens, origin, resourcesTokenController);
        }

        public async Task PayResource(Investigator investigator, int amount, Card toCard)
        {
            TokenController resourcesTokenController = _areaInvestigatorViewsManager.Get(investigator).ResourcesTokenController;
            List<TokenView> allTokens = _tokensGeneratorComponent.GetResourceTokens(amount);
            Transform destiny = GetTransformFor(toCard);
            await _swapInvestigatorPresenter.Select(investigator);
            await Pay(allTokens, destiny, resourcesTokenController);
        }

        private Transform GetTransformFor(Card card) =>
            card.IsSpecial ? _tokensGeneratorComponent.ShowToken : _cardViewsManager.Get(card).transform;

        private async Task Gain(List<TokenView> allTokens, Transform origin, TokenController tokenController)
        {
            await MoveUp(allTokens, origin, null);
            await MoveDown(allTokens, tokenController.TokenOff.transform, () => tokenController.AddToken(1));
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
