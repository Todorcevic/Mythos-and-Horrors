using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01556 : CardCondition
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Fortune, Tag.Insight };

        protected override Task ExecuteConditionEffect(Investigator investigator)
        {
            throw new System.NotImplementedException();
        }
    }
}
