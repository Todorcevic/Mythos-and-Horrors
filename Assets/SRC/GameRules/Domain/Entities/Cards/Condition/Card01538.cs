using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01538 : CardCondition
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Tactic, Tag.Insight };

        protected override Task ExecuteConditionEffect()
        {
            throw new System.NotImplementedException();
        }
    }
}
