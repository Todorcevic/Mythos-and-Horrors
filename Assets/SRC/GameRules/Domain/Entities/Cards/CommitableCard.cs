using System.Diagnostics.CodeAnalysis;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CommitableCard : Card
    {
        public State Commited { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Commited = CreateState(false);
        }

        /*******************************************************************/
        public int GetChallengeValue(ChallengeType challengeType)
        {
            int wildAmount = (Info.Wild ?? 0);
            return challengeType switch
            {
                ChallengeType.Strength => wildAmount + (Info.Strength ?? 0),
                ChallengeType.Agility => wildAmount + (Info.Agility ?? 0),
                ChallengeType.Intelligence => wildAmount + (Info.Intelligence ?? 0),
                ChallengeType.Power => wildAmount + (Info.Power ?? 0),
                _ => wildAmount
            };
        }
    }
}
