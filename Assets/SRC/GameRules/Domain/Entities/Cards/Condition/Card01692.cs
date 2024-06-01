using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01692 : CardCondition
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Fortune, Tag.Blessed };

        protected override Task ExecuteConditionEffect()
        {
            throw new System.NotImplementedException();
        }
    }
}
