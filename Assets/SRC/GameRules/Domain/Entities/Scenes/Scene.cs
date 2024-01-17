using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public abstract class Scene : IStartReactionable, IEndReactionable
    {
        [Inject] public SceneInfo Info { get; }
        public Zone DangerDeckZone { get; } = new Zone();
        public Zone DangerDiscardZone { get; } = new Zone();
        public Zone GoalZone { get; } = new Zone();
        public Zone PlotZone { get; } = new Zone();
        public Zone VictoryZone { get; } = new Zone();
        public Zone LimboZone { get; } = new Zone();
        public Zone OutZone { get; } = new Zone();
        public Zone SelectorZone { get; } = new Zone();
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
                    PlaceZone[i, j] = new Zone();
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
