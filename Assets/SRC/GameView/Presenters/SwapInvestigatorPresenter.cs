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

        private Investigator _investigatorSelected;
        private Sequence _sequence = DOTween.Sequence();

        /*******************************************************************/
        public async Task Select(Investigator investigator)
        {
            if (_investigatorSelected == investigator) return;
            await _sequence.AsyncWaitForCompletion();
            _sequence = DOTween.Sequence()
            .Join(_swapInvestigatorComponent.Select(investigator))
            .Join(_avatarViewsManager.Get(_investigatorSelected ?? investigator).Deselect())
            .Join(_avatarViewsManager.Get(investigator).Select());
            _investigatorSelected = investigator;
            await _sequence.AsyncWaitForCompletion();
        }

        public async Task Select(Zone zone)
        {
            Investigator investigator = _investigatorsProvider.GetInvestigatorWithThisZone(zone) ?? _investigatorSelected;
            await Select(investigator);
        }
    }
}
