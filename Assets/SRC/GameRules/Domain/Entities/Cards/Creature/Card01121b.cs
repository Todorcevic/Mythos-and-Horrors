using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01121b : CardCreature, IStalker, ITarget, ISpawnable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly BuffsProvider _buffsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public IReaction AvoidPayHintReaction { get; private set; }
        public IReaction AvoidGainHintReaction { get; private set; }

        public Investigator TargetInvestigator => _investigatorsProvider.AllInvestigatorsInPlay
           .OrderByDescending(investigator => investigator.Hints.Value).First();
        public CardPlace SpawnPlace => TargetInvestigator.CurrentPlace;
        public override IEnumerable<Tag> Tags => new[] { Tag.Humanoid, Tag.Cultist, Tag.Elite };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            RemoveStat(Health);
            Health = CreateStat((Info.Health ?? 0) + _investigatorsProvider.AllInvestigators.Count() * 2);
            CreateBuff(CardsToBuff, CantGainAndPayHintsBuff, RemoveCantGainAndPayHintsBuff);
            AvoidGainHintReaction = CreateReaction<GainHintGameAction>(CantGainHintsCondition, CantGainHintsLogic, isAtStart: true);
            AvoidGainHintReaction.Disable();
            AvoidPayHintReaction = CreateReaction<PayHintsToGoalGameAction>(CantPayHintsCondition, CantPayHintsLogic, isAtStart: true);
            AvoidPayHintReaction.Disable();
        }

        /*******************************************************************/
        private IEnumerable<Card> CardsToBuff() =>
           ConfrontedInvestigator != null ? new List<Card> { ConfrontedInvestigator.InvestigatorCard } : Enumerable.Empty<Card>();

        private async Task CantGainAndPayHintsBuff(IEnumerable<Card> cards)
        {
            AvoidGainHintReaction.Enable();
            CantPayHintsLogic();
            AvoidPayHintReaction.Enable();
            await Task.CompletedTask;
        }

        private async Task RemoveCantGainAndPayHintsBuff(IEnumerable<Card> cards)
        {
            AvoidGainHintReaction.Disable();
            DisableCantPayHintsLogic();
            AvoidPayHintReaction.Disable();
            await Task.CompletedTask;
        }

        /*******************************************************************/
        bool CantGainHintsCondition(GainHintGameAction gainHintGameAction)
        {
            if (gainHintGameAction.Investigator != ConfrontedInvestigator) return false;
            return true;
        }

        async Task CantGainHintsLogic(GainHintGameAction gainHintGameAction)
        {
            gainHintGameAction.Cancel();
            await Task.CompletedTask;
        }

        bool CantPayHintsCondition(PayHintsToGoalGameAction payHintToGoalGameAction)
        {
            if (!payHintToGoalGameAction.InvestigatorsToPay.Contains(ConfrontedInvestigator)) return false;
            return true;
        }

        async Task CantPayHintsLogic(PayHintsToGoalGameAction payHintToGoalGameAction)
        {
            IEnumerable<Investigator> investigatorsToPay = payHintToGoalGameAction.InvestigatorsToPay.Except(new[] { ConfrontedInvestigator });
            payHintToGoalGameAction.UpdateInvestigatorsToPay(investigatorsToPay);
            await Task.CompletedTask;
        }

        /*******************************************************************/
        void CantPayHintsLogic()
        {
            GameConditionWith<Investigator> payCondition = _chaptersProvider.CurrentScene.CurrentGoal.PayHints.Condition;
            payCondition.AddCondition(NewPayCondition);

            bool NewPayCondition(Investigator investigator)
            {
                if (_investigatorsProvider.AllInvestigatorsInPlay.Except(new[] { ConfrontedInvestigator })
                    .Sum(investigator => investigator.Hints.Value) < _chaptersProvider.CurrentScene.CurrentGoal.Hints.Value) return false;
                return true;
            }
        }

        void DisableCantPayHintsLogic()
        {
            _chaptersProvider.CurrentScene.CurrentGoal.PayHints.Condition.Reset();
        }
    }
}
