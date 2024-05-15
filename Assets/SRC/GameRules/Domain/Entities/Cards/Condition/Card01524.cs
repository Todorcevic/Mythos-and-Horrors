using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01524 : CardCondition
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Tactic };


        public override Task ExecuteConditionEffect()
        {
            throw new System.NotImplementedException();
        }
    }
}
