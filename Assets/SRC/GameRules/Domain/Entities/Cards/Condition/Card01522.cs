using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01522 : CardCondition
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Insight, Tag.Criminal };


        public override Task ExecuteConditionEffect()
        {
            throw new System.NotImplementedException();
        }
    }
}
