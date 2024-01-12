using DG.Tweening;
using MythsAndHorrors.GameRules;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class SwapInvestigatorPresenter
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;
        [Inject] private readonly SwapInvestigatorComponent _swapInvestigatorComponent;
        [Inject] private readonly ActivatorUIPresenter _activatorUIPresenter;

        private Investigator _investigatorSelected;

        /*******************************************************************/
        public async Task Select(Investigator investigator)
        {
            if (_investigatorSelected == investigator) return;
            _investigatorSelected = investigator;
            _activatorUIPresenter.Deactivate();
            Sequence dfasd = DOTween.Sequence();
            dfasd.Join(_swapInvestigatorComponent.Select(investigator));
            dfasd.Join(_avatarViewsManager.Get(investigator).Select());
            dfasd.Join(_avatarViewsManager.Get(_investigatorSelected)?.Deselect());
            await dfasd.AsyncWaitForCompletion();
            _activatorUIPresenter.Activate();
        }

        public async Task Select(Zone zone)
        {
            Investigator investigator = _investigatorsProvider.GetInvestigatorWithThisZone(zone) ?? _investigatorSelected;
            await Select(investigator);
        }
    }
}
