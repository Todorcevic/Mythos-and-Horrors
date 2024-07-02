using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01510 : CardConditionFast
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Tactic };
        public State Protected { get; private set; }
        protected override GameActionTime FastReactionAtStart => GameActionTime.Before;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Protected = new State(false);
            CreateForceReaction<RoundGameAction>(RemovePlayedCondition, RemovePlayedLogic, GameActionTime.Before);
            CreateForceReaction<CreatureAttackGameAction>(CancelAttackCreatureCondition, CancelAttackCreaturePlayedLogic, GameActionTime.Initial);
            CreateBuff(CardsToBuff, ActivationBuff, DeactivationBuff);
        }

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Protected, true).Execute();
        }

        protected override bool CanPlayFromHandSpecific(GameAction gameAction)
        {
            if (gameAction is not PlayInvestigatorGameAction playInvestigatorGameAction) return false;
            if (playInvestigatorGameAction.ActiveInvestigator != ControlOwner) return false;
            return true;
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
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Protected, false).Execute();
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
    }
}
