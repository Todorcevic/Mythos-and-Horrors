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
            if (investigator.Hints.Value < 1)
                await _gameActionsProvider.Create(new DrawDangerGameAction(investigator));
            else
            {
                ChallengePhaseGameAction challengeGameAction = null;
                challengeGameAction = new(investigator.Intelligence, 4, "Pista false", this, failEffect: DropHints);
                await _gameActionsProvider.Create(challengeGameAction);

                async Task DropHints()
                {
                    await _gameActionsProvider.Create<DropHintGameAction>()
                        .SetWith(investigator, investigator.CurrentPlace.Hints, challengeGameAction.ResultChallenge.TotalDifferenceValue * -1)
                        .Start();
                }
            }
        }
    }
}
