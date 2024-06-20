using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01525 : CardTalent
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Practiced };

        /*******************************************************************/
        public override bool TalentCondition(ChallengePhaseGameAction challengePhaseGameAction)
        {
            if (challengePhaseGameAction is not AttackCreatureGameAction attackCreatureGameAction) return false;
            if (!attackCreatureGameAction.IsSuccessful ?? true) return false;
            return true;
        }

        public override async Task TalentLogic(ChallengePhaseGameAction challengePhaseGameAction)
        {
            if (challengePhaseGameAction is not AttackCreatureGameAction attackCreatureGameAction) return;
            attackCreatureGameAction.UpdateAmountDamage(attackCreatureGameAction.AmountDamage + 1);
            await Task.CompletedTask;
        }
    }
}
