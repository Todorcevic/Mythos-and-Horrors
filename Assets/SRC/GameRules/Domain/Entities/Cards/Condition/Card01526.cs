using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01526 : CardCondition
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Supply };

        public override Task ExecuteConditionEffect()
        {
            throw new System.NotImplementedException();
        }
    }
}
