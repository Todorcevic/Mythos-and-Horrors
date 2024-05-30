using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01579 : CardCondition
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Fortune };

        public override async Task ExecuteConditionEffect()
        {
            await Task.CompletedTask;
        }
    }
}
