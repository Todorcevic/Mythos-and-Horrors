using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public abstract class Scene
    {
        [Inject] private readonly ZonesProvider _zonesProvider;

        [Inject] public SceneInfo Info { get; }
        public Zone DangerDeckZone { get; private set; }
        public Zone DangerDiscardZone { get; private set; }
        public Zone GoalZone { get; private set; }
        public Zone PlotZone { get; private set; }
        public Zone VictoryZone { get; private set; }
        public Zone LimboZone { get; private set; }
        public Zone OutZone { get; private set; }
        public Zone[,] PlaceZone { get; } = new Zone[3, 7];
        public CardPlot CurrentPlot => PlotZone.Cards.Last() as CardPlot;
    

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Zenject injects this method")]
        private void Init()
        {
            DangerDeckZone = _zonesProvider.Create();
            DangerDiscardZone = _zonesProvider.Create();
            GoalZone = _zonesProvider.Create();
            PlotZone = _zonesProvider.Create();
            VictoryZone = _zonesProvider.Create();
            LimboZone = _zonesProvider.Create();
            OutZone = _zonesProvider.Create();
            InitializePlaceZones();
        }

        /*******************************************************************/
        public abstract Task PrepareScene();

        private void InitializePlaceZones()
        {
            for (int i = 0; i < PlaceZone.GetLength(0); i++)
            {
                for (int j = 0; j < PlaceZone.GetLength(1); j++)
                {
                    PlaceZone[i, j] = _zonesProvider.Create();
                }
            }
        }

        public bool HasThisZone(Zone zone)
        {
            return zone == DangerDeckZone
                || zone == DangerDiscardZone
                || zone == GoalZone
                || zone == PlotZone
                || zone == VictoryZone
                || zone == LimboZone
                || zone == OutZone
                || PlaceZone.Cast<Zone>().Contains(zone);
        }
    }
}
