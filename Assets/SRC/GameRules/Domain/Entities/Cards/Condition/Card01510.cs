using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01510 : CardCondition
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Tactic };
        public State Protected { get; private set; }
        protected override bool IsFast => true;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Protected = new State(false);
            CreateReaction<RoundGameAction>(RemovePlayedCondition, RemovePlayedLogic, isAtStart: true);
            CreateReaction<CreatureAttackGameAction>(CancelAttackCreatureCondition, CancelAttackCreaturePlayedLogic, isAtStart: true);
            CreateBuff(CardsToBuff, ActivationBuff, DeactivationBuff);
        }

        /*******************************************************************/
        private async Task CancelAttackCreaturePlayedLogic(CreatureAttackGameAction creatureAttackGameAction)
        {
            creatureAttackGameAction.Cancel();
            await Task.CompletedTask;
        }

        private bool CancelAttackCreatureCondition(CreatureAttackGameAction creatureAttackGameAction)
        {
            if (!Protected.IsActive) return false;
            if (creatureAttackGameAction.Investigator != ControlOwner) return false;
            if (creatureAttackGameAction.Creature.HasThisTag(Tag.Elite)) return false;
            return true;
        }

        /*******************************************************************/
        private async Task RemovePlayedLogic(RoundGameAction action)
        {
            await _gameActionsProvider.Create(new UpdateStatesGameAction(Protected, false));
        }

        private bool RemovePlayedCondition(RoundGameAction action)
        {
            if (!Protected.IsActive) return false;
            return true;
        }

        /*******************************************************************/
        private async Task DeactivationBuff(IEnumerable<Card> enumerable) => await Task.CompletedTask;

        private async Task ActivationBuff(IEnumerable<Card> enumerable) => await Task.CompletedTask;

        private IEnumerable<Card> CardsToBuff() =>
            Protected.IsActive ? new List<CardInvestigator>() { Owner.InvestigatorCard } : Enumerable.Empty<Card>();

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(Investigator investigator)
        {
            await _gameActionsProvider.Create(new UpdateStatesGameAction(Protected, true));
        }

        protected override bool CanPlayFromHandSpecific(GameAction gameAction) => true;
    }
}
