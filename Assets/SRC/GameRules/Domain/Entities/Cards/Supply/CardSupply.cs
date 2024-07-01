using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{

    public class CardSupply : CommitableCard, IPlayableFromHand
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Stat ResourceCost { get; private set; }
        public Stat PlayFromHandTurnsCost { get; protected set; }
        public GameConditionWith<Investigator> PlayFromHandCondition { get; private set; }
        public GameCommand<GameAction> PlayFromHandCommand { get; private set; }
        public PlayActionType PlayFromHandActionType => PlayActionType.PlayFromHand;

        /*******************************************************************/
        public CardPlace CurrentPlace => IsInPlay ? ControlOwner?.CurrentPlace : null;

        public bool HasAnyOfThisSlots(IEnumerable<SlotType> slotsType) => Info.Slots.Intersect(slotsType).Any();

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            ResourceCost = CreateStat(Info.Cost ?? 0);
            PlayFromHandTurnsCost = CreateStat(1);
            PlayFromHandCondition = new GameConditionWith<Investigator>(ConditionToPlayFromHand);
            PlayFromHandCommand = new GameCommand<GameAction>(PlayFromHand);
        }

        /*******************************************************************/
        private bool ConditionToPlayFromHand(Investigator investigator)
        {
            if (CurrentZone.ZoneType != ZoneType.Hand) return false;
            if (ControlOwner != investigator) return false;
            if (ResourceCost.Value > ControlOwner.Resources.Value) return false;
            if (PlayFromHandTurnsCost.Value > ControlOwner.CurrentTurns.Value) return false;
            return true;
        }

        public async Task PlayFromHand(GameAction investigator)
        {
            await _gameActionsProvider.Create<MoveCardsGameAction>()
                .SetWith(this, ControlOwner.AidZone).Start();
        }
    }
}
