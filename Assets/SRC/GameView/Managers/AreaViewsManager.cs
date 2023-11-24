using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class AreaViewsManager
    {
        [Inject] private readonly List<AreaAdventurerView> _allAreas;

        /*******************************************************************/
        public AreaAdventurerView Get(Adventurer adventurer) => _allAreas.Find(areaView => areaView.Adventurer == adventurer);
    }
}
