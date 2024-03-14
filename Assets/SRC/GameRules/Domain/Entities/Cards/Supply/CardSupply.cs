using System.Diagnostics.CodeAnalysis;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CardSupply : Card, IPlayableFromHand
    {
        public Stat ResourceCost { get; private set; }
        public Stat TurnsCost { get; private set; }
        public Stat Health { get; private set; }
        public Stat Sanity { get; private set; }
        public int TotalChallengePoints => (Info.Strength ?? 0) + (Info.Agility ?? 0) + (Info.Intelligence ?? 0) + (Info.Power ?? 0) + (Info.Wild ?? 0);

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            ResourceCost = new Stat(Info.Cost ?? 0);
            TurnsCost = new Stat(1);
            Health = new Stat(Info.Health ?? 0, Info.Health ?? 0);
            Sanity = new Stat(Info.Sanity ?? 0, Info.Sanity ?? 0);
        }
    }
}
