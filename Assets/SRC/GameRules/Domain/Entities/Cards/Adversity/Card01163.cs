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
            ChallengePhaseGameAction challengeGameAction = _gameActionsProvider.Create<ChallengePhaseGameAction>();
            await challengeGameAction.SetWith(investigator.Power, 3, "Challenge: " + Info.Name, this, failEffect: FailEffect).Start();

            /*******************************************************************/
            async Task FailEffect() =>
                await _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(investigator, this, amountFear: challengeGameAction.ResultChallenge.TotalDifferenceValue * -1).Start();

        }
    }
}
