using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01584 : CardCondition
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Fortune };

        public override Task ExecuteConditionEffect()
        {
            throw new System.NotImplementedException();
        }
    }
}
