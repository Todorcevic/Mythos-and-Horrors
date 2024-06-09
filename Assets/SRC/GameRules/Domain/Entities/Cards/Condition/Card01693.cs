using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01693 : CardCondition
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Supply };

        protected override bool IsFast => false;

        protected override Task ExecuteConditionEffect(Investigator investigator)
        {
            throw new System.NotImplementedException();
        }
    }
}
