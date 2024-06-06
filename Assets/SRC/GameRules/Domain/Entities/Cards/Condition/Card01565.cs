using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01565 : CardCondition
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Spell, Tag.Spirit };

        protected override Task ExecuteConditionEffect(Investigator investigator)
        {
            throw new System.NotImplementedException();
        }
    }
}
