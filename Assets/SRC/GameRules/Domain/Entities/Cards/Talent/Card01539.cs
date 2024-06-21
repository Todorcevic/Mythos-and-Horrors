using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01539 : CardTalent
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Practiced };

        /*******************************************************************/
        public override bool TalentCondition(ChallengePhaseGameAction challengePhaseGameAction)
        {
            if (challengePhaseGameAction is not InvestigatePlaceGameAction investigatePlaceGameAction) return false;
            if (!investigatePlaceGameAction.IsSucceed) return false;
            return true;
        }

        public override async Task TalentLogic(ChallengePhaseGameAction challengePhaseGameAction)
        {
            if (challengePhaseGameAction is not InvestigatePlaceGameAction investigatePlaceGameAction) return;
            investigatePlaceGameAction.UpdateAmountHints(investigatePlaceGameAction.AmountHints + 1);
            await Task.CompletedTask;
        }
    }
}
