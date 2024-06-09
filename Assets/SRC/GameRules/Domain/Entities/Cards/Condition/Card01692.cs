using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01692 : CardCondition
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Fortune, Tag.Blessed };

        protected override bool IsFast => false;

        protected override Task ExecuteConditionEffect(Investigator investigator)
        {
            throw new System.NotImplementedException();
        }
    }
}
