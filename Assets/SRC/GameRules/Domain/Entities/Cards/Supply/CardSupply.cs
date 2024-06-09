using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CardSupply : Card, IPlayableFromHand, ICommitable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Stat ResourceCost { get; private set; }
        public Stat PlayFromHandTurnsCost { get; protected set; }

        public GameCondition<GameAction> PlayFromHandCondition { get; private set; }
        public GameCommand<PlayFromHandGameAction> PlayFromHandCommand { get; private set; }

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
            PlayFromHandCondition = new GameCondition<GameAction>(ConditionToPlayFromHand);
            PlayFromHandCommand = new GameCommand<PlayFromHandGameAction>(PlayFromHand);
        }

        /*******************************************************************/
        int ICommitable.GetChallengeValue(ChallengeType challengeType)
        {
            int wildAmount = Info.Wild ?? 0;
            if (challengeType == ChallengeType.Strength) return wildAmount + Info.Strength ?? 0;
            if (challengeType == ChallengeType.Agility) return wildAmount + Info.Agility ?? 0;
            if (challengeType == ChallengeType.Intelligence) return wildAmount + Info.Intelligence ?? 0;
            if (challengeType == ChallengeType.Power) return wildAmount + Info.Power ?? 0;
            return wildAmount;
        }

        /*******************************************************************/
        private bool ConditionToPlayFromHand(GameAction gameAction)
        {
            if (gameAction is not OneInvestigatorTurnGameAction oneInvestigatorTurnGameAction) return false;
            if (CurrentZone.ZoneType != ZoneType.Hand) return false;
            if (ControlOwner != oneInvestigatorTurnGameAction.ActiveInvestigator) return false;
            if (ResourceCost.Value > ControlOwner.Resources.Value) return false;
            if (PlayFromHandTurnsCost.Value > ControlOwner.CurrentTurns.Value) return false;
            return true;
        }

        private async Task PlayFromHand(PlayFromHandGameAction playFromHandGameAction)
        {
            await _gameActionsProvider.Create(new MoveCardsGameAction(this, playFromHandGameAction.Investigator.AidZone));
        }
    }
}
