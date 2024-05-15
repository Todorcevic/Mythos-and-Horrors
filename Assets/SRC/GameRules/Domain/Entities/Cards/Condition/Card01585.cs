using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01585 : CardCondition
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Spirit };

        public override Task ExecuteConditionEffect()
        {
            throw new System.NotImplementedException();
        }
    }
}
