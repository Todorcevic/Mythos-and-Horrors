using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class Scene
    {
        [Inject] private readonly ZonesProvider _zonesProvider;
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorProvider;
        [Inject] private readonly EffectsProvider _effectProvider;
        [Inject] private readonly GameActionProvider _gameActionFactory;
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;

        [Inject] public SceneInfo Info { get; }
        public Zone DangerDeckZone { get; private set; }
        public Zone DangerDiscardZone { get; private set; }
        public Zone GoalZone { get; private set; }
        public Zone PlotZone { get; private set; }
        public Zone VictoryZone { get; private set; }
        public Zone LimboZone { get; private set; }
        public Zone OutZone { get; private set; }
        public Zone[,] PlaceZone { get; } = new Zone[3, 7];
        public CardPlot CurrentPlot => PlotZone.Cards.LastOrDefault() as CardPlot;
        public CardGoal CurrentGoal => GoalZone.Cards.LastOrDefault() as CardGoal;
        public Card CardDangerToDraw => DangerDeckZone.Cards.LastOrDefault();

        /************************** RESOURCES *****************************/
        public Stat PileAmount { get; private set; }

        /*******************************************************************/
        [Inject]
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
            PileAmount = new Stat(int.MaxValue);
        }

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

        /*******************************************************************/
        public abstract Task PrepareScene();

        /*******************************************************************/
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
