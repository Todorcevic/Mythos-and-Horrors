using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01163 : CardAdversityLimbo
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Terror };

        /*******************************************************************/
        protected override async Task ObligationLogic(Investigator investigator)
        {
            ChallengePhaseGameAction challengeGameAction = null;
            challengeGameAction = new(investigator.Power, 3, "Challenge: " + Info.Name, this, failEffect: FailEffect);
            await _gameActionsProvider.Create(challengeGameAction);

            async Task FailEffect()
            {
                await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(investigator, this, amountFear: challengeGameAction.TotalDifferenceValue * -1));
            }
        }
    }
}
