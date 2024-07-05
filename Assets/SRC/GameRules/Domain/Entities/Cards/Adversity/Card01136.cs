using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01136 : CardAdversityLimbo
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        /*******************************************************************/
        protected override async Task ObligationLogic(Investigator investigator)
        {
            if (investigator.Hints.Value < 1) //No have hints
                await _gameActionsProvider.Create<DrawDangerGameAction>().SetWith(investigator).Execute();
            else
            {
                ChallengePhaseGameAction challengeGameAction = _gameActionsProvider.Create<ChallengePhaseGameAction>();
                await challengeGameAction.SetWith(investigator.Intelligence, 4, "Pista false", this, failEffect: DropHints).Execute();

                /*******************************************************************/
                async Task DropHints()
                {
                    await _gameActionsProvider.Create<DropHintGameAction>()
                        .SetWith(investigator, investigator.CurrentPlace.Hints, challengeGameAction.ResultChallenge.TotalDifferenceValue * -1)
                        .Execute();
                }
            }
        }
    }
}
