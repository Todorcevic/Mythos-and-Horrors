using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01583 : CardCondition
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Fortune };

        protected override Task ExecuteConditionEffect()
        {
            throw new System.NotImplementedException();
        }
    }
}
