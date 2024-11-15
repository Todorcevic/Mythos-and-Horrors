﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class Scene : SceneInfo
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ZonesProvider _zonesProvider;

        public Zone DangerDeckZone => Zones.First(zone => zone.ZoneType == ZoneType.DangerDeck);
        public Zone DangerDiscardZone => Zones.First(zone => zone.ZoneType == ZoneType.DangerDiscard);
        public Zone GoalZone => Zones.First(zone => zone.ZoneType == ZoneType.Goal);
        public Zone PlotZone => Zones.First(zone => zone.ZoneType == ZoneType.Plot);
        public Zone VictoryZone => Zones.First(zone => zone.ZoneType == ZoneType.Victory);
        public Zone LimboZone => Zones.First(zone => zone.ZoneType == ZoneType.Limbo);
        public Zone OutZone => _zonesProvider.OutZone;
        public Zone GetPlaceZone(int row, int column) => Zones.FindAll(zone => zone.ZoneType == ZoneType.Place)[row * 7 + column];
        public CardPlot CurrentPlot => PlotZone.Cards.LastOrDefault() as CardPlot;
        public CardGoal CurrentGoal => GoalZone.Cards.LastOrDefault() as CardGoal;
        public CardPlot FirstPlot => PlotCards.First();
        public CardGoal FirstGoal => GoalCards.First();
        public Card CardDangerToDraw => DangerDeckZone.Cards.LastOrDefault();
        public abstract IEnumerable<Card> StartDeckDangerCards { get; }

        /************************** TOKENS *****************************/
        public ChallengeToken GetNewStarToken()
        {
            return new ChallengeToken(ChallengeTokenType.Star, effect: StarEffect, value: StarValue, description: StarDescription);

            /*******************************************************************/
            async Task StarEffect(Investigator investigator) => await investigator.InvestigatorCard.StarTokenEffect.Invoke();
            int StarValue(Investigator investigator) => investigator.InvestigatorCard.StarTokenValue.Invoke();
            string StarDescription(Investigator investigator) => investigator.InvestigatorCard.StarTokenDescription.Invoke();
        }

        public ChallengeToken GetNewFailToken()
        {
            return new ChallengeToken(ChallengeTokenType.Fail, effect: FailEffect, description: (_) => FailTokenDescription);

            /*******************************************************************/
            async Task FailEffect(Investigator investigator)
            {
                _gameActionsProvider.CurrentChallenge.IsAutoFail = true;
                await Task.CompletedTask;
            }

        }
        public abstract ChallengeToken GetNewAncientToken();
        public abstract ChallengeToken GetNewCultistToken();
        public abstract ChallengeToken GetNewDangerToken();
        public abstract ChallengeToken GetNewCreatureToken();

        /************************** RESOURCES *****************************/
        public Stat PileAmount { get; private set; }

        /************************* RESOLUTIONS ****************************/
        public List<Resolution> FullResolutions { get; } = new();

        /*******************************************************************/
        public virtual void Init()
        {
            PileAmount = new Stat(int.MaxValue, canBeNegative: false);
            Zones.Add(new(ZoneType.DangerDeck));
            Zones.Add(new(ZoneType.DangerDiscard));
            Zones.Add(new(ZoneType.Goal));
            Zones.Add(new(ZoneType.Plot));
            Zones.Add(new(ZoneType.Victory));
            Zones.Add(new(ZoneType.Limbo));
            InitializePlaceZones();
            PrepareResolutions();
            _reactionablesProvider.CreateReaction<EliminateInvestigatorGameAction>(InvestigatorsLooseCondition, InvestigatorsLooseLogic, GameActionTime.After);
        }

        /*******************************************************************/
        private void InitializePlaceZones()
        {
            const int AMOUNT_PLACES = 21;
            for (int i = 0; i < AMOUNT_PLACES; i++)
            {
                Zones.Add(new(ZoneType.Place));
            }
        }

        /*******************************************************************/
        private async Task InvestigatorsLooseLogic(EliminateInvestigatorGameAction action)
        {
            await _gameActionsProvider.Create<FinalizeGameAction>().SetWith(FullResolutions[0]).Execute();
        }

        private bool InvestigatorsLooseCondition(EliminateInvestigatorGameAction action)
        {
            if (_investigatorsProvider.AllInvestigatorsInPlay.Count() > 0) return false;
            return true;
        }

        /*******************************************************************/
        public abstract Task PrepareScene();

        private void PrepareResolutions()
        {
            for (int i = 0; i < ResolutionHistories.Count; i++)
            {
                FullResolutions.Add(new Resolution(ResolutionHistories[i], GetResolution(i)));
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
