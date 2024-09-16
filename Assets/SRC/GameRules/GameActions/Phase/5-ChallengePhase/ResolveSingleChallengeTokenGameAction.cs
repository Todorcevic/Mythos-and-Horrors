using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ResolveSingleChallengeTokenGameAction : GameAction
    {
        public ChallengeToken ChallengeTokenToResolve { get; private set; }
        public Investigator Investigator { get; private set; }
        public override bool CanBeExecuted => Investigator.IsInPlay.IsTrue && ChallengeTokenToResolve.Effect != null;

        /*******************************************************************/
        public ResolveSingleChallengeTokenGameAction SetWith(ChallengeToken challengeTokenToResolve, Investigator investigator)
        {
            ChallengeTokenToResolve = challengeTokenToResolve;
            Investigator = investigator;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await ChallengeTokenToResolve.Effect.Invoke(Investigator);
        }
    }
}
