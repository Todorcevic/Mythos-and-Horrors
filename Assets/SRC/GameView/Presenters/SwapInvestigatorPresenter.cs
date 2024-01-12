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

        /*******************************************************************/
        public async Task Select(Investigator investigator)
        {
            if (_swapInvestigatorComponent.InvestigatorSelected == investigator) return;
            _activatorUIPresenter.Deactivate();
            await DOTween.Sequence()
                 .Join(_swapInvestigatorComponent.Select(investigator))
                 .Join(_avatarViewsManager.Get(investigator).Select())
                 .Join(_avatarViewsManager.Get(_swapInvestigatorComponent.InvestigatorSelected).Deselect())
                 .AsyncWaitForCompletion();
            _activatorUIPresenter.Activate();
        }

        public async Task Select(Zone zone)
        {
            Investigator investigator = _investigatorsProvider.GetInvestigatorWithThisZone(zone) ?? _swapInvestigatorComponent.InvestigatorSelected;
            await Select(investigator);
        }
    }
}
