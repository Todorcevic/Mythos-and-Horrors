using System.Collections.Generic;

namespace MythosAndHorrors.GameRules
{
    public class Card01161 : CardCreature, ITarget
    {
        public Investigator TargetInvestigator => throw new System.NotImplementedException();
        public override IEnumerable<Tag> Tags => new[] { Tag.Ghoul };
    }

}
