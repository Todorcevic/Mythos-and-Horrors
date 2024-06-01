using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01522 : CardCondition
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Insight, Tag.Criminal };


        protected override async Task ExecuteConditionEffect()
        {
            await Task.CompletedTask;
        }
    }
}
