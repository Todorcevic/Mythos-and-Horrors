using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class ChallengeActivation : ITriggered
    {
        public Card Card { get; }
        public Stat ActivateTurnsCost { get; }
        public GameCommand<ChallengePhaseGameAction> Logic { get; }
        public GameConditionWith<ChallengePhaseGameAction> Condition { get; }
        public PlayActionType PlayAction { get; }
        public string Description { get; }
        public bool IsDisable { get; private set; }
        public bool IsFreeActivation => ActivateTurnsCost.Value < 1;

        /*******************************************************************/
        public ChallengeActivation(Card card, Stat activateTurnsCost, GameCommand<ChallengePhaseGameAction> logic, GameConditionWith<ChallengePhaseGameAction> condition, PlayActionType playActionType)
        {
            Card = card;
            ActivateTurnsCost = activateTurnsCost;
            Logic = logic;
            Condition = condition;
            PlayAction = PlayActionType.Activate | playActionType;
        }

        /*******************************************************************/
        public bool FullCondition(ChallengePhaseGameAction challenge)
        {
            if (IsDisable) return false;
            if (!Condition.IsTrueWith(challenge)) return false;
            return true;
        }

        public async Task PlayFor(ChallengePhaseGameAction investigator)
        {
            await Logic.RunWith(investigator);
        }

        public void Enable() => IsDisable = false;

        public void Disable() => IsDisable = true;
    }
}
