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
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;

        public Investigator TargetInvestigator => _investigatorsProvider.AllInvestigatorsInPlay
           .OrderByDescending(investigator => investigator.Hints.Value).First();

        public CardPlace SpawnPlace => TargetInvestigator.CurrentPlace;

        public override IEnumerable<Tag> Tags => new[] { Tag.Humanoid, Tag.Cultist, Tag.Elite };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Health = CreateStat((Info.Health ?? 0) + _investigatorsProvider.AllInvestigators.Count * 2);
            _buffsProvider.Create()
               .SetCard(this)
               .SetDescription(nameof(CantGainAndPayHintsBuff))
               .SetCardsToBuff(CardsToBuff)
               .SetAddBuff(CantGainAndPayHintsBuff)
               .SetRemoveBuff(RemoveCantGainAndPayHintsBuff);
        }

        /*******************************************************************/
        private IEnumerable<Card> CardsToBuff() =>
           ConfrontedInvestigator != null ? new List<Card> { ConfrontedInvestigator.InvestigatorCard } : Enumerable.Empty<Card>();

        private async Task CantGainAndPayHintsBuff(IEnumerable<Card> cards)
        {
            _reactionablesProvider.CreateReaction<GainHintGameAction>(CantGainHintsCondition, CantGainHintsLogic, isAtStart: true);
            await Task.CompletedTask;

            _reactionablesProvider.CreateReaction<PayHintsToGoalGameAction>(CantPayHintsCondition, CantPayHintsLogic, isAtStart: true);
            await Task.CompletedTask;
        }

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
            Effect payHintEffect = payHintToGoalGameAction.EffectsToPay.Find(effect => effect.Card == ConfrontedInvestigator.AvatarCard);
            payHintToGoalGameAction.RemoveEffect(payHintEffect);
            await Task.CompletedTask;
        }

        /*******************************************************************/
        private async Task RemoveCantGainAndPayHintsBuff(IEnumerable<Card> cards)
        {
            _reactionablesProvider.RemoveReaction<GainHintGameAction>(CantGainHintsLogic);
            _reactionablesProvider.RemoveReaction<PayHintsToGoalGameAction>(CantPayHintsLogic);
            await Task.CompletedTask;
        }
    }
}
