using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01578 : CardCondition
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Tactic };

        protected override bool IsFast => false;

        protected override Task ExecuteConditionEffect(Investigator investigator)
        {
            throw new System.NotImplementedException();
        }

        protected override bool CanPlayFromHandSpecific(GameAction gameAction) => true;

    }
}
