using System.Collections.Generic;
using System.Linq;

namespace MythosAndHorrors.GameRules
{
    public class CardAvatar : Card
    {
        public override IEnumerable<Tag> Tags => Enumerable.Empty<Tag>();
        public override CardInfo Info => Owner.InvestigatorCard.Info;
    }
}
