using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01592 : CardTalent
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Innate };

        /*******************************************************************/
        public override bool TalentCondition(ChallengePhaseGameAction challengePhaseGameAction)
        {

            return true;
        }

        public override async Task TalentLogic(ChallengePhaseGameAction challengePhaseGameAction)
        {
            await Task.CompletedTask;
        }

    }
}
