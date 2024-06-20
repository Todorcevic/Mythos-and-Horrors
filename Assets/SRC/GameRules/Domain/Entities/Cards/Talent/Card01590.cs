using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01590 : CardTalent
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Practiced };

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
