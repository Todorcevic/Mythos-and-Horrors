using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01567 : CardTalent
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Innate };

        /*******************************************************************/
        public override bool TalentCondition(ChallengePhaseGameAction challengePhaseGameAction)
        {
            if (!challengePhaseGameAction.IsSucceed) return false;
            return true;
        }

        public override async Task TalentLogic(ChallengePhaseGameAction challengePhaseGameAction)
        {
            challengePhaseGameAction.SuccesEffects.Add(HealthFear);
            await Task.CompletedTask;

            async Task HealthFear() =>
                await _gameActionsProvider.Create<HealthGameAction>().SetWith(InvestigatorCommiter.InvestigatorCard, amountFearToRecovery: 1).Execute();
        }
    }
}
