using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01685 : CardConditionPlayFromHand
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Insight };

        protected override bool IsFast => false;

        protected override Task ExecuteConditionEffect(Investigator investigator)
        {
            throw new System.NotImplementedException();
        }

        protected override bool CanPlayFromHandSpecific(GameAction gameAction) => true;

    }
}
