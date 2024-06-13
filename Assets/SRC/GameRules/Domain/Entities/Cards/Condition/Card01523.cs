using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01523 : CardConditionTrigged
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Tactic };
        protected override bool FastReactionAtStart => true;

        /*******************************************************************/
        protected override bool CanPlayFromHandSpecific(GameAction gameAction)
        {
            if (gameAction is not CreatureAttackGameAction creatureAttackGameAction) return false;
            if (!ControlOwner.CurrentPlace.InvestigatorsInThisPlace.Contains(creatureAttackGameAction.Investigator)) return false;
            return true;
        }

        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            if (gameAction is not CreatureAttackGameAction creatureAttackGameAction) return;
            creatureAttackGameAction.Cancel();
            await Task.CompletedTask;
        }
    }
}
