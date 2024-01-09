using Zenject;
using System.Threading.Tasks;
using MythsAndHorrors.GameRules;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;

namespace MythsAndHorrors.GameView
{
    public class ResourceMoverPresenter : IResourceMover
    {
        [Inject(Id = ViewValues.CENTER_SHOW_POSITION)] private readonly Transform _centerShowPosition;
        [Inject] private readonly AreaInvestigatorViewsManager _areaInvestigatorViewsManager;
        [Inject] private readonly TokensGeneratorComponent _tokensGeneratorComponent;
        [Inject] private readonly SwapInvestigatorPresenter _swapInvestigatorPresenter;

        /*******************************************************************/
        public async Task AddResource(Investigator investigator, int amount)
        {
            TokenController resourceTokenController = _areaInvestigatorViewsManager.Get(investigator).ResourcesTokenController;
            List<TokenView> allTokens = _tokensGeneratorComponent.GetResourceTokens(amount);

            await _swapInvestigatorPresenter.Select(investigator);
            await MoveUp(allTokens, _tokensGeneratorComponent.ShowToken.position, null);
            await MoveDown(allTokens, resourceTokenController.TokenOff.transform, Finish);

            void Finish() => resourceTokenController.AddToken(1);
        }

        public async Task RemoveResource(Investigator investigator, int amount)
        {
            TokenController resourceTokenController = _areaInvestigatorViewsManager.Get(investigator).ResourcesTokenController;
            List<TokenView> allTokens = _tokensGeneratorComponent.GetResourceTokens(amount);

            await _swapInvestigatorPresenter.Select(investigator);
            await MoveUp(allTokens, resourceTokenController.TokenOn.transform.position, Prepare);
            await MoveDown(allTokens, _tokensGeneratorComponent.ShowToken, null);

            void Prepare() => resourceTokenController.TokenOn.SetAmount(0);
        }

        private async Task MoveUp(List<TokenView> allTokens, Vector3 startPosition, TweenCallback starting)
        {
            Sequence sequence = DOTween.Sequence();
            foreach (TokenView tokenView in allTokens)
            {
                Vector3 randomPosition = (Random.insideUnitSphere * 5f) + Vector3.up * Random.Range(-0.2f, 0.2f);
                sequence.Join(DOTween.Sequence()
                .PrependCallback(() => tokenView.transform.position = startPosition)
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
