using System.Diagnostics.CodeAnalysis;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CardSupply : Card, IPlayableFromHand, ICommitable
    {
        public Stat ResourceCost { get; private set; }
        public Stat TurnsCost { get; protected set; }
        public Stat Health { get; private set; }
        public Stat Sanity { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            ResourceCost = new Stat(Info.Cost ?? 0);
            TurnsCost = new Stat(1);
            Health = new Stat(Info.Health ?? 0);
            Sanity = new Stat(Info.Sanity ?? 0);
        }

        /*******************************************************************/
        public int GetChallengeValue(ChallengeType challengeType)
        {
            int amount = Info.Wild ?? 0;
            if (challengeType == ChallengeType.Strength) return amount + Info.Strength ?? 0;
            if (challengeType == ChallengeType.Agility) return amount + Info.Agility ?? 0;
            if (challengeType == ChallengeType.Intelligence) return amount + Info.Intelligence ?? 0;
            if (challengeType == ChallengeType.Power) return amount + Info.Power ?? 0;
            return amount;
        }
    }
}
