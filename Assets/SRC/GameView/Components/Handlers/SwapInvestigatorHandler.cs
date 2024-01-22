using DG.Tweening;
using MythsAndHorrors.GameRules;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class SwapInvestigatorHandler
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;
        [Inject] private readonly SwapInvestigatorComponent _swapInvestigatorComponent;
        [Inject] private readonly IOActivatorComponent _ioActivatorComponent;
        private Investigator _investigatorSelected;

        /*******************************************************************/
        public async Task Select(Investigator investigator, bool withActivation = false)
        {
            if (_investigatorSelected == investigator) return;
            await _ioActivatorComponent.DeactivateSensor();
            await DOTween.Sequence()
            .Join(_swapInvestigatorComponent.Select(investigator))
            .Join(_avatarViewsManager.Get(_investigatorSelected ?? investigator).Deselect())
            .Join(_avatarViewsManager.Get(investigator).Select()).AsyncWaitForCompletion();
            _investigatorSelected = investigator;
            if (withActivation) _ioActivatorComponent.ActivateSensor();
        }

        public async Task Select(Zone zone)
        {
            Investigator investigator = _investigatorsProvider.GetInvestigatorWithThisZone(zone) ?? _investigatorSelected;
            await Select(investigator);
        }
    }
}
