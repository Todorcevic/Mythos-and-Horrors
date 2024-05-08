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
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ZonesProvider _zonesProvider;
        private readonly List<IReaction> _reactions = new();

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
        public CardPlot FirstPlot => Info.PlotCards.First();
        public CardGoal FirstGoal => Info.GoalCards.First();
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

        /************************* RESOLUTIONS ****************************/
        public List<Resolution> Resolutions { get; } = new();

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
            PileAmount = new Stat(int.MaxValue, canBeNegative: false);
            InitializePlaceZones();
            PrepareChallengeTokens();
            PrepareResolutions();
            _reactionablesProvider.SubscribeAtStart(WhenBegin);
            _reactionablesProvider.SubscribeAtEnd(WhenFinish);
            CreateReaction<EliminateInvestigatorGameAction>(InvestigatorsLooseCondition, InvestigatorsLooseLogic, isAtStart: false);
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
        private async Task InvestigatorsLooseLogic(EliminateInvestigatorGameAction action)
        {
            await _gameActionsProvider.Create(new FinalizeGameAction(Resolutions[0])); //TODO: Debe ser corregido, la resolucion esta mal diseñada
        }

        private bool InvestigatorsLooseCondition(EliminateInvestigatorGameAction action)
        {
            if (_investigatorsProvider.AllInvestigatorsInPlay.Count() > 0) return false;
            return true;
        }

        /*******************************************************************/
        public abstract Task PrepareScene();

        protected virtual void PrepareChallengeTokens()
        {
            StarToken = new ChallengeToken(ChallengeTokenType.Star, effect: StarEffect, value: StarValue);
            FailToken = new ChallengeToken(ChallengeTokenType.Fail, effect: FailEffect);

            async Task StarEffect() => await _gameActionsProvider.CurrentChallenge.ActiveInvestigator.InvestigatorCard.StarTokenEffect();

            int StarValue() => _gameActionsProvider.CurrentChallenge.ActiveInvestigator.InvestigatorCard.StarTokenValue();

            async Task FailEffect()
            {
                _gameActionsProvider.CurrentChallenge.IsAutoFail = true;
                await Task.CompletedTask;
            }
        }

        private void PrepareResolutions()
        {
            for (int i = 0; i < Info.Resolutions.Count; i++)
            {
                Resolutions.Add(new Resolution(Info.Resolutions[i], GetResolution(i)));
            }
        }

        /*******************************************************************/
        protected Reaction<T> FindReactionByLogic<T>(Func<T, Task> logic) where T : GameAction =>
           _reactions.Find(reaction => reaction is Reaction<T> reactionT && reactionT.Logic == logic) as Reaction<T>;

        protected void RemoveReaction<T>(Func<T, Task> logic) where T : GameAction =>
            _reactions.RemoveAll(reaction => reaction is Reaction<T> reactionT && reactionT.Logic == logic);

        protected Reaction<T> CreateReaction<T>(Func<T, bool> condition, Func<T, Task> logic, bool isAtStart, bool isBase = false)
           where T : GameAction
        {
            Reaction<T> newReaction = new(condition, logic, isAtStart, isBase);
            _reactions.Add(newReaction);
            return newReaction;
        }

        private async Task WhenBegin(GameAction gameAction)
        {
            foreach (IReaction reaction in _reactions.FindAll(reaction => reaction.IsAtStart))
                await reaction.React(gameAction);
        }

        private async Task WhenFinish(GameAction gameAction)
        {
            foreach (IReaction reaction in _reactions.FindAll(reaction => !reaction.IsAtStart))
                await reaction.React(gameAction);
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
