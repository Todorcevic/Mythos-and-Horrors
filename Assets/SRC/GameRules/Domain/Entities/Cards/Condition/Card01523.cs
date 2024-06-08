using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01523 : CardConditionFast
    {
        private CreatureAttackGameAction _currentCreatureAttackGameAction;

        public override IEnumerable<Tag> Tags => new[] { Tag.Tactic };
        protected override bool FastReactionAtStart => true;

        /*******************************************************************/
        protected override bool CanPlayFromHandOverride(GameAction gameAction)
        {
            if (gameAction is not CreatureAttackGameAction creatureAttackGameAction) return false;
            if (!ControlOwner.CurrentPlace.InvestigatorsInThisPlace.Contains(creatureAttackGameAction.Investigator)) return false;
            _currentCreatureAttackGameAction = creatureAttackGameAction;
            return true;
        }

        protected override Task ExecuteConditionEffect(Investigator investigator)
        {
            _currentCreatureAttackGameAction.Cancel();
            return Task.CompletedTask;
        }
    }
}
