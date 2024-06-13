using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardCondition : Card, ICommitable
    {
        public Stat ResourceCost { get; private set; }
        public Stat PlayFromHandTurnsCost { get; protected set; }
        public State Commited { get; private set; }
        public virtual PlayActionType PlayFromHandActionType => PlayActionType.PlayFromHand;
        public GameCondition<GameAction> PlayFromHandCondition { get; private set; }
        protected abstract bool IsFast { get; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            ResourceCost = CreateStat(Info.Cost ?? 0);
            PlayFromHandTurnsCost = CreateStat(IsFast ? 0 : 1);
            Commited = CreateState(false);
            PlayFromHandCondition = new GameCondition<GameAction>(CanPlayFromHandWith);
        }

        /*******************************************************************/
        int ICommitable.GetChallengeValue(ChallengeType challengeType)
        {
            int wildAmount = Info.Wild ?? 0;
            return challengeType switch
            {
                ChallengeType.Strength => wildAmount + Info.Strength ?? 0,
                ChallengeType.Agility => wildAmount + Info.Agility ?? 0,
                ChallengeType.Intelligence => wildAmount + Info.Intelligence ?? 0,
                ChallengeType.Power => wildAmount + Info.Power ?? 0,
                _ => wildAmount
            };
        }

        /*******************************************************************/
        protected abstract bool CanPlayFromHandWith(GameAction gameAction);

        public abstract Task PlayFromHand(GameAction investigator);
    }
}
