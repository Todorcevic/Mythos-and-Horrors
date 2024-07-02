using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01525 : CardTalent
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Practiced };

        /*******************************************************************/
        public override bool TalentCondition(ChallengePhaseGameAction challengePhaseGameAction)
        {
            if (challengePhaseGameAction is not AttackCreatureGameAction attackCreatureGameAction) return false;
            if (!attackCreatureGameAction.IsSucceed) return false;
            return true;
        }

        public override async Task TalentLogic(ChallengePhaseGameAction challengePhaseGameAction)
        {
            if (challengePhaseGameAction is not AttackCreatureGameAction attackCreatureGameAction) return;
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(attackCreatureGameAction.AmountDamage, 1).Execute();
        }
    }
}
