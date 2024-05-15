using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01513 : CardCondition
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Spell, Tag.Weakness };

        public override Task ExecuteConditionEffect()
        {
            throw new System.NotImplementedException();
        }
    }
}
