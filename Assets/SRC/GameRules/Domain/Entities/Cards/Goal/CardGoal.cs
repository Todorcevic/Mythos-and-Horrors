﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardGoal : Card, IRevealable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProviders;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;

        public Stat Keys { get; protected set; }
        public State Revealed { get; private set; }
        public Activation<Investigator> PayKeys { get; protected set; }
        public GameCommand<RevealGameAction> RevealCommand { get; private set; }
        public Reaction<UpdateStatGameAction> Reveal { get; private set; }
        public override IEnumerable<Tag> Tags => Enumerable.Empty<Tag>();

        /*******************************************************************/
        public int Position => _chaptersProviders.CurrentScene.GoalCards.IndexOf(this);
        public CardGoal NextCardGoal => _chaptersProviders.CurrentScene.GoalCards.NextElementFor(this);
        public int MaxKeys => (Info.Keys ?? 0) * _investigatorsProvider.AllInvestigators.Count();
        public int AmountOfKeys => MaxKeys - Keys.Value;
        public bool IsComplete => Revealed.IsActive;
        public History InitialHistory => Info.Histories?.ElementAt(0) ?? new History(); //TODO: Remove control when all cards have history
        public History RevealHistory => Info.Histories?.ElementAt(1) ?? new History(); //TODO: Remove control when all cards have history

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            Keys = CreateStat(MaxKeys);
            PayKeys = CreateFastActivation(PayKeysActivate, PayKeysConditionToActivate, PlayActionType.Activate, new Localization("Activation_CardGoal", () => new[] { Keys.Value.ToString() }));
            RevealCommand = new GameCommand<RevealGameAction>(RevealEffect);
            Revealed = CreateState(false);
            Reveal = CreateBaseReaction<UpdateStatGameAction>(RevealCondition, RevealLogic, GameActionTime.After);
        }

        /*******************************************************************/
        protected bool RevealCondition(UpdateStatGameAction updateStatGameAction)
        {
            if (!updateStatGameAction.HasThisStat(Keys)) return false;
            if (IsInPlay.IsFalse) return false;
            if (Revealed.IsActive) return false;
            if (Keys.Value > 0) return false;
            return true;
        }

        protected async Task RevealLogic(UpdateStatGameAction updateStatGameAction) =>
            await _gameActionsProvider.Create<RevealGameAction>().SetWith(this).Execute();

        /*******************************************************************/
        private async Task RevealEffect(RevealGameAction revealGameAction)
        {
            await _gameActionsProvider.Create<ShowHistoryGameAction>().SetWith(RevealHistory, this).Execute();
            await CompleteEffect();
            await _gameActionsProvider.Create<DiscardGameAction>().SetWith(this).Execute();
            await _gameActionsProvider.Create<PlaceGoalGameAction>().SetWith(NextCardGoal).Execute();
        }

        protected abstract Task CompleteEffect();

        /*******************************************************************/
        protected virtual async Task PayKeysActivate(Investigator activeInvestigator)
        {
            await _gameActionsProvider.Create<SafeWhile>().SetWith(Condition, PayKey).Execute();

            /*******************************************************************/
            bool Condition() => _investigatorsProvider.AllInvestigatorsInPlay.Any(investigator => investigator.CanPayKeys.IsTrue) && IsInPlay.IsTrue && !Revealed.IsActive && Keys.Value > 0;
            async Task PayKey()
            {
                IEnumerable<Investigator> specificInvestigators = _investigatorsProvider.AllInvestigatorsInPlay
                        .Where(investigator => investigator.CanPayKeys.IsTrue);
                await _gameActionsProvider.Create<PayKeysToGoalGameAction>().SetWith(this, specificInvestigators).Execute();
            }
        }

        protected virtual bool PayKeysConditionToActivate(Investigator activeInvestigator)
        {
            if (IsInPlay.IsFalse) return false;
            if (Revealed.IsActive) return false;
            if (_investigatorsProvider.AllInvestigatorsInPlay.Where(investigator => investigator.CanPayKeys.IsTrue)
                .Sum(investigator => investigator.Keys.Value) < Keys.Value) return false;
            return true;
        }
    }
}
