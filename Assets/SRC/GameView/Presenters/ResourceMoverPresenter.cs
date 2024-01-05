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
        [Inject(Id = ViewValues.CENTER_SHOW_POSITION)] private Transform _centerShowPosition;
        [Inject] private readonly CardViewsManager _cardsManager;
        [Inject] private readonly TokensGeneratorComponent _tokensGeneratorComponent;

        /*******************************************************************/
        public async Task AddResource(Investigator investigator, int amount)
        {
            InvestigatorCardView investigatorCardView = _cardsManager.Get(investigator.InvestigatorCard) as InvestigatorCardView;
            List<TokenView> allTokens = _tokensGeneratorComponent.GetResourceTokens(amount);

            Sequence sequence = DOTween.Sequence();
            sequence.OnStart(Prepare);
            allTokens.ForEach(token => sequence.Join(token.transform.DOFullMove(_centerShowPosition)));
            sequence.AppendInterval(0f);
            allTokens.ForEach(token => sequence.Join(token.transform.DOFullMove(investigatorCardView.ResourceTokenOff.transform)));
            sequence.OnComplete(Finish);
            await sequence.AsyncWaitForCompletion();

            void Prepare()
            {
                foreach (TokenView tokenView in allTokens)
                {
                    tokenView.transform.position = _tokensGeneratorComponent.transform.position;
                    tokenView.Activate();
                }
            }

            void Finish()
            {
                allTokens.ForEach(token => token.Deactivate());
                investigatorCardView.ResourceTokenOff.Activate();
            }
        }

        public async Task RemoveResource(Investigator investigator, int amount)
        {
            InvestigatorCardView investigatorCardView = _cardsManager.Get(investigator.InvestigatorCard) as InvestigatorCardView;
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
                    tokenView.transform.position = investigatorCardView.ResourceTokenOn.transform.position;
                    tokenView.Activate();
                    investigatorCardView.ResourceTokenOn.Deactivate();
                }
            }

            void Finish() => allTokens.ForEach(token => token.Deactivate());
        }
    }
}
