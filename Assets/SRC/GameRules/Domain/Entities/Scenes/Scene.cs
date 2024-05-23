using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Scene : SceneInfo
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ZonesProvider _zonesProvider;

        //public List<Zone> Zones { get; init; } = new();
        //[Inject] public SceneInfo Info { get; }
        public Zone DangerDeckZone => Zones.First(zone => zone.ZoneType == ZoneType.DangerDeck);
        public Zone DangerDiscardZone => Zones.First(zone => zone.ZoneType == ZoneType.DangerDiscard);
        public Zone GoalZone => Zones.First(zone => zone.ZoneType == ZoneType.Goal);
        public Zone PlotZone => Zones.First(zone => zone.ZoneType == ZoneType.Plot);
        public Zone VictoryZone => Zones.First(zone => zone.ZoneType == ZoneType.Victory);
        public Zone LimboZone => Zones.First(zone => zone.ZoneType == ZoneType.Limbo);
        public Zone OutZone => _zonesProvider.OutZone;
        public Zone[,] PlaceZone { get; } = new Zone[3, 7];
        public CardPlot CurrentPlot => PlotZone.Cards.LastOrDefault() as CardPlot;
        public CardGoal CurrentGoal => GoalZone.Cards.LastOrDefault() as CardGoal;
        public CardPlot FirstPlot => PlotCards.First();
        public CardGoal FirstGoal => GoalCards.First();
        public Card CardDangerToDraw => DangerDeckZone.Cards.LastOrDefault();
        public virtual IEnumerable<Card> StartDeckDangerCards { get; }

        /************************** TOKENS *****************************/
        public ChallengeToken StarToken { get; protected set; }
        public ChallengeToken FailToken { get; protected set; }
        public ChallengeToken AncientToken { get; protected set; }
        public ChallengeToken CultistToken { get; protected set; }
        public ChallengeToken DangerToken { get; protected set; }
        public ChallengeToken CreatureToken { get; protected set; }

        /************************** RESOURCES *****************************/
        public Stat PileAmount { get; private set; }

        /************************* RESOLUTIONS ****************************/
        public List<Resolution> FullResolutions { get; } = new();

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Zones.Add(_zonesProvider.Create(ZoneType.DangerDeck));
            Zones.Add(_zonesProvider.Create(ZoneType.DangerDiscard));
            Zones.Add(_zonesProvider.Create(ZoneType.Goal));
            Zones.Add(_zonesProvider.Create(ZoneType.Plot));
            Zones.Add(_zonesProvider.Create(ZoneType.Victory));
            Zones.Add(_zonesProvider.Create(ZoneType.Limbo));
            PileAmount = new Stat(int.MaxValue, canBeNegative: false);
            InitializePlaceZones();
            PrepareDefaultChallengeTokens();
            PrepareResolutions();
            _reactionablesProvider.CreateReaction<EliminateInvestigatorGameAction>(InvestigatorsLooseCondition, InvestigatorsLooseLogic, isAtStart: false);
        }

        /*******************************************************************/
        private void InitializePlaceZones()
        {
            for (int i = 0; i < PlaceZone.GetLength(0); i++)
            {
                for (int j = 0; j < PlaceZone.GetLength(1); j++)
                {
                    PlaceZone[i, j] = _zonesProvider.Create(ZoneType.Place);
                }
            }
        }

        /*******************************************************************/
        private async Task InvestigatorsLooseLogic(EliminateInvestigatorGameAction action)
        {
            await _gameActionsProvider.Create(new FinalizeGameAction(FullResolutions[0]));
        }

        private bool InvestigatorsLooseCondition(EliminateInvestigatorGameAction action)
        {
            if (_investigatorsProvider.AllInvestigatorsInPlay.Count() > 0) return false;
            return true;
        }

        /*******************************************************************/
        public virtual async Task PrepareScene() { await Task.CompletedTask; }
        protected virtual void PrepareChallengeTokens() { }

        private void PrepareDefaultChallengeTokens()
        {
            PrepareChallengeTokens();
            StarToken = new ChallengeToken(ChallengeTokenType.Star, effect: StarEffect, value: StarValue);
            FailToken = new ChallengeToken(ChallengeTokenType.Fail, effect: FailEffect);

            /*******************************************************************/
            async Task StarEffect(Investigator investigator) => await investigator.InvestigatorCard.StarTokenEffect();

            int StarValue(Investigator investigator) => investigator.InvestigatorCard.StarTokenValue();

            async Task FailEffect(Investigator investigator)
            {
                _gameActionsProvider.CurrentChallenge.IsAutoFail = true;
                await Task.CompletedTask;
            }
        }

        private void PrepareResolutions()
        {
            for (int i = 0; i < Resolutions.Count; i++)
            {
                FullResolutions.Add(new Resolution(Resolutions[i], GetResolution(i)));
            }
        }

        /*******************************************************************/
        protected virtual async Task Resolution0() => await Task.CompletedTask;
        protected virtual async Task Resolution1() => await Task.CompletedTask;
        protected virtual async Task Resolution2() => await Task.CompletedTask;
        protected virtual async Task Resolution3() => await Task.CompletedTask;
        protected virtual async Task Resolution4() => await Task.CompletedTask;
        protected virtual async Task Resolution5() => await Task.CompletedTask;
        protected virtual async Task Resolution6() => await Task.CompletedTask;
        protected virtual async Task Resolution7() => await Task.CompletedTask;

        private Func<Task> GetResolution(int index) => index switch
        {
            0 => Resolution0,
            1 => Resolution1,
            2 => Resolution2,
            3 => Resolution3,
            4 => Resolution4,
            5 => Resolution5,
            6 => Resolution6,
            7 => Resolution7,
            _ => throw new ArgumentOutOfRangeException(nameof(index), "Index out range."),
        };
    }
}
