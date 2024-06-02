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
        public Stat PlayFromHandTurnsCost { get; private set; }
        public Stat Health { get; private set; }
        public Stat Sanity { get; private set; }
        public GameCondition<GameAction> PlayFromHandCondition { get; private set; }
        public GameCommand<PlayFromHandGameAction> PlayFromHandCommand { get; private set; }

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
            if (this is IDamageable) Health = CreateStat(Info.Health ?? 0);
            if (this is IFearable) Sanity = CreateStat(Info.Sanity ?? 0);
            CreateReaction<UpdateStatGameAction>(DefeatCondition, DefeatLogic, false);
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
        private bool DefeatCondition(UpdateStatGameAction gameAction)
        {
            if (!IsInPlay) return false;
            if (!DieByDamage() && !DieByFear()) return false;
            return true;

            bool DieByDamage()
            {
                if (this is not IDamageable damageable) return false;
                if (damageable.Health.Value > 0) return false;
                return true; ;
            }

            bool DieByFear()
            {
                if (this is not IFearable fearable) return false;
                if (fearable.Sanity.Value > 0) return false;
                return true;
            }
        }

        private async Task DefeatLogic(UpdateStatGameAction updateStatGameAction)
        {
            Card byThisCard = null;
            if (updateStatGameAction.Parent is HarmToCardGameAction harmToCardGameAction) byThisCard = harmToCardGameAction.ByThisCard;
            await _gameActionsProvider.Create(new DefeatCardGameAction(this, byThisCard));
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
