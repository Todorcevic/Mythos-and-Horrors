using Zenject;
using System.Threading.Tasks;
using MythsAndHorrors.GameRules;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

namespace MythsAndHorrors.GameView
{
    public class ResourceMoverPresenter : IResourceMover
    {
        [Inject(Id = ViewValues.CENTER_SHOW_POSITION)] private readonly Transform _centerShowPosition;
        [Inject] private readonly SwapInvestigatorComponent _swapInvestigatorComponent;
        [Inject] private readonly TokensGeneratorComponent _tokensGeneratorComponent;

        /*******************************************************************/
        public async Task AddResource(Investigator investigator, int amount)
        {
            TokenController resourceTokenController = _swapInvestigatorComponent.Get(investigator).ResourcesTokenController;
            List<TokenView> allTokens = _tokensGeneratorComponent.GetResourceTokens(amount);

            Sequence sequence = DOTween.Sequence();
            sequence.OnStart(Prepare);
            allTokens.ForEach(token => sequence.Join(token.transform.DOFullMove(_centerShowPosition)));
            sequence.AppendInterval(0f);
            allTokens.ForEach(token => sequence.Join(token.transform.DOFullMove(resourceTokenController.TokenOff.transform)));
            sequence.OnComplete(Finish);
            await sequence.AsyncWaitForCompletion();

            void Prepare()
            {
                foreach (TokenView tokenView in allTokens)
                {
                    tokenView.transform.position = _tokensGeneratorComponent.transform.position;
                    tokenView.SetAmount(1);
                }
            }

            void Finish()
            {
                allTokens.ForEach(token => token.SetAmount(0));
                resourceTokenController.AddToken(amount);
            }
        }

        public async Task RemoveResource(Investigator investigator, int amount)
        {
            TokenController resourceTokenController = _swapInvestigatorComponent.Get(investigator).ResourcesTokenController;
            List<TokenView> allTokens = _tokensGeneratorComponent.GetResourceTokens(amount);

            Sequence sequence = DOTween.Sequence();
            sequence.OnStart(Prepare);
            allTokens.ForEach(token => sequence.Join(token.transform.DOFullMove(_centerShowPosition)));
            sequence.AppendInterval(0f);
            allTokens.ForEach(token => sequence.Join(token.transform.DOFullMove(_tokensGeneratorComponent.transform)));
            sequence.OnComplete(Finish);
            await sequence.AsyncWaitForCompletion();

            void Prepare()
            {
                foreach (TokenView tokenView in allTokens)
                {
                    tokenView.transform.position = resourceTokenController.TokenOn.transform.position;
                    tokenView.SetAmount(1);
                    resourceTokenController.TokenOn.SetAmount(0);
                }
            }

            void Finish() => allTokens.ForEach(token => token.SetAmount(0));
        }
    }
}
