using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CardCondition : Card, IPlayableFromHand
    {
        [Inject] private readonly GameActionProvider _gameActionFactory;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public Stat ResourceCost { get; private set; }
        public Stat TurnsCost { get; private set; }

        public int TotalChallengePoints => (Info.Strength ?? 0) + (Info.Agility ?? 0) + (Info.Intelligence ?? 0) + (Info.Power ?? 0) + (Info.Wild ?? 0);

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            ResourceCost = new Stat(Info.Cost ?? 0);
            TurnsCost = new Stat(1);
        }

        /*******************************************************************/
        public bool CanPlayFromHand()
        {
            if (!Owner.HandZone.Cards.Contains(this)) return false;
            if (Owner.Resources.Value < ResourceCost.Value) return false;
            if (Owner.Turns.Value < TurnsCost.Value) return false;
            return true;
        }

        public async Task PlayFromHand()
        {
            await _gameActionFactory.Create(new DiscardGameAction(this));
        }
    }
}
