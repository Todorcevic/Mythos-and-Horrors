using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ResolveMultiChallengeTokensGamaAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public List<ChallengeToken> ChallengeTokensToResolve { get; private set; }
        public override bool CanBeExecuted => ChallengeTokensToResolve.Count > 0;

        /*******************************************************************/
        public ResolveMultiChallengeTokensGamaAction(List<ChallengeToken> challengeTokensToResolve)
        {
            ChallengeTokensToResolve = challengeTokensToResolve;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            ChallengeToken toResolve = ChallengeTokensToResolve.First();
            await _gameActionsProvider.Create(new ResolveSingleChallengeTokenGameAction(toResolve));
            ChallengeTokensToResolve.Remove(toResolve);
            await _gameActionsProvider.Create(new ResolveMultiChallengeTokensGamaAction(ChallengeTokensToResolve));
        }
    }
}
