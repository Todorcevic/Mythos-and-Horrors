using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class Scene
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
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
        public CardPlot CurrentPlot => PlotZone.Cards.LastOrDefault() as CardPlot;
        public CardGoal CurrentGoal => GoalZone.Cards.LastOrDefault() as CardGoal;
        public Card CardDangerToDraw => DangerDeckZone.Cards.LastOrDefault();

        /************************** TOKENS *****************************/
        public ChallengeToken StarToken { get; protected set; }
        public ChallengeToken FailToken { get; protected set; }
        public ChallengeToken AncientToken { get; protected set; }
        public ChallengeToken CultistToken { get; protected set; }
        public ChallengeToken DangerToken { get; protected set; }
        public ChallengeToken CreatureToken { get; protected set; }

        /************************** RESOURCES *****************************/
        public Stat PileAmount { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
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
            PrepareChallengeTokens();
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

        public virtual void PrepareChallengeTokens()
        {
            StarToken = new ChallengeToken(ChallengeTokenType.Star, effect: StarEffect, value: StarValue);
            FailToken = new ChallengeToken(ChallengeTokenType.Fail, effect: FailEffect);

            async Task StarEffect() => await _gameActionsProvider.CurrentChallenge.ActiveInvestigator.InvestigatorCard.StarEffect();

            int StarValue() => _gameActionsProvider.CurrentChallenge.ActiveInvestigator.InvestigatorCard.StarValue();

            async Task FailEffect()
            {
                _gameActionsProvider.CurrentChallenge.IsAutoFail = true;
                await Task.CompletedTask;
            }
        }

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
