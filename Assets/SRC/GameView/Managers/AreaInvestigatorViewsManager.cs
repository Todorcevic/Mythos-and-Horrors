using MythosAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class AreaInvestigatorViewsManager
    {
        [Inject] private readonly List<AreaInvestigatorView> _allAreaInvestigatorViews;

        /*******************************************************************/
        public void Init(List<Investigator> investigators)
        {
            investigators.ForEach(investigator =>
            _allAreaInvestigatorViews.OrderBy(areaInvestigatorView => areaInvestigatorView.name).First(area => area.IsFree).Init(investigator));
        }

        /*******************************************************************/
        public AreaInvestigatorView Get(Investigator investigator) =>
            _allAreaInvestigatorViews.First(areaInvestigatorView => areaInvestigatorView.Investigator == investigator);
    }
}
