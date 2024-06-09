using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01579 : CardCondition
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Fortune };

        protected override bool IsFast => false;

        protected override async Task ExecuteConditionEffect(Investigator investigator)
        {
            await Task.CompletedTask;
        }

        protected override bool CanPlayFromHandSpecific(GameAction gameAction) => true;

    }
}
