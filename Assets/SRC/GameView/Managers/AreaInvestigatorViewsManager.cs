using MythosAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class AreaInvestigatorViewsManager
    {
        [Inject] private readonly List<AreaInvestigatorView> _allAreaInvestigatorViews;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        /*******************************************************************/
        public void Init()
        {
            _investigatorsProvider.AllInvestigators.ForEach(investigator =>
            _allAreaInvestigatorViews.OrderBy(areaInvestigatorView => areaInvestigatorView.name).
            First(area => area.IsFree).Init(investigator));
        }

        /*******************************************************************/
        public AreaInvestigatorView Get(Investigator investigator) =>
            _allAreaInvestigatorViews.First(areaInvestigatorView => areaInvestigatorView.Investigator == investigator);
    }
}
