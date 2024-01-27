using DG.Tweening;
using MythsAndHorrors.GameRules;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class SwapInvestigatorHandler
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;
        [Inject] private readonly SwapInvestigatorComponent _swapInvestigatorComponent;
        private Investigator _investigatorSelected;

        /*******************************************************************/
        public Sequence Select(Investigator investigator)
        {
            if (_investigatorSelected == investigator) return DOTween.Sequence();
            Sequence swapSequence = DOTween.Sequence()
            .Join(_swapInvestigatorComponent.Select(investigator))
            .Join(_avatarViewsManager.Get(_investigatorSelected ?? investigator).Deselect())
            .Join(_avatarViewsManager.Get(investigator).Select());
            _investigatorSelected = investigator;
            return swapSequence;
        }

        public Sequence Select(Zone zone)
        {
            Investigator investigator = _investigatorsProvider.GetInvestigatorWithThisZone(zone) ?? _investigatorSelected;
            return Select(investigator);
        }
    }
}
