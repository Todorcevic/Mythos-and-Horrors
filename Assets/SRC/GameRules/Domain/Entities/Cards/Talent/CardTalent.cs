
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardTalent : CommitableCard
    {
        public GameConditionWith<ChallengePhaseGameAction> Condition { get; private set; }
        public GameCommand<ChallengePhaseGameAction> Logic { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Condition = new GameConditionWith<ChallengePhaseGameAction>(TalentCondition);
            Logic = new GameCommand<ChallengePhaseGameAction>(TalentLogic);
        }

        /*******************************************************************/
        public abstract bool TalentCondition(ChallengePhaseGameAction challengePhaseGameAction);

        public abstract Task TalentLogic(ChallengePhaseGameAction challengePhaseGameAction);

        protected override void BlankState(bool isActive)
        {
            base.BlankState(isActive);
            if (isActive)
            {
                Condition.Disable();
                Logic.Disable();
            }
            else
            {
                Condition.Enable();
                Logic.Enable();
            }
        }
    }
}
