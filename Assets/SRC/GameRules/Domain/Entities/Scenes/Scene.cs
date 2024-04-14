using System;
using System.Collections.Generic;
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
            DangerDeckZone = _zonesProvider.Create(ZoneType.DangerDeck);
            DangerDiscardZone = _zonesProvider.Create(ZoneType.DangerDiscard);
            GoalZone = _zonesProvider.Create(ZoneType.Goal);
            PlotZone = _zonesProvider.Create(ZoneType.Plot);
            VictoryZone = _zonesProvider.Create(ZoneType.Victory);
            LimboZone = _zonesProvider.Create(ZoneType.Limbo);
            OutZone = _zonesProvider.Create(ZoneType.Out);
            PileAmount = new Stat(int.MaxValue);
            InitializePlaceZones();
            PrepareChallengeTokens();
        }

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
        public abstract Task PrepareScene();

        protected virtual void PrepareChallengeTokens()
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
        public virtual async Task Resolution0() => await Task.CompletedTask;
        public virtual async Task Resolution1() => await Task.CompletedTask;
        public virtual async Task Resolution2() => await Task.CompletedTask;
        public virtual async Task Resolution3() => await Task.CompletedTask;
        public virtual async Task Resolution4() => await Task.CompletedTask;
        public virtual async Task Resolution5() => await Task.CompletedTask;
        public virtual async Task Resolution6() => await Task.CompletedTask;
        public virtual async Task Resolution7() => await Task.CompletedTask;
    }
}
