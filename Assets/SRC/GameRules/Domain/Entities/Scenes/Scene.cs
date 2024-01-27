using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public abstract class Scene : IStartReactionable, IEndReactionable
    {
        [Inject] public SceneInfo Info { get; }
        public Zone DangerDeckZone { get; } = new Zone(ZoneType.DangerDeck);
        public Zone DangerDiscardZone { get; } = new Zone(ZoneType.DangerDiscard);
        public Zone GoalZone { get; } = new Zone(ZoneType.Goal);
        public Zone PlotZone { get; } = new Zone(ZoneType.Plot);
        public Zone VictoryZone { get; } = new Zone(ZoneType.Victory);
        public Zone LimboZone { get; } = new Zone(ZoneType.Limbo);
        public Zone OutZone { get; } = new Zone(ZoneType.Out);
        public Zone[,] PlaceZone { get; } = new Zone[3, 7];
        public CardPlot CurrentPlot => PlotZone.Cards.Last() as CardPlot;
        public Stat ResourcesPile { get; } = new Stat(int.MaxValue);

        /*******************************************************************/
        protected Scene()
        {
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
                    PlaceZone[i, j] = new Zone(ZoneType.Place);
                }
            }
        }

        /*********************** Resources Logic ****************************/
        public virtual async Task WhenBegin(GameAction gameAction)
        {
            await Task.CompletedTask;
        }

        public virtual async Task WhenFinish(GameAction gameAction)
        {
            await Task.CompletedTask;
        }
    }
}
