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
            if (investigator.Keys.Value < 1) //No have hints
                await _gameActionsProvider.Create<DrawDangerGameAction>().SetWith(investigator).Execute();
            else
            {
                ChallengePhaseGameAction challengeGameAction = _gameActionsProvider.Create<ChallengePhaseGameAction>();
                await challengeGameAction.SetWith(investigator.Intelligence, 4, new Localization("Challenge_Card01136", Info.Name), this, failEffect: DropKeys).Execute();

                /*******************************************************************/
                async Task DropKeys()
                {
                    await _gameActionsProvider.Create<DropKeyGameAction>()
                        .SetWith(investigator, investigator.CurrentPlace.Keys, challengeGameAction.ResultChallenge.TotalDifferenceValue * -1)
                        .Execute();
                }
            }
        }
    }
}
