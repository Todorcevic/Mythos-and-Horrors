using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardConditionTrigged : CardCondition
    {
        protected abstract bool FastReactionAtStart { get; }
        protected override bool IsFast => true;

        public Reaction<GameAction> PlayFromHand { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            PlayFromHand = CreateOptativeReaction<GameAction>(PlayFromHandCondition.IsTrueWith,
                PlayFromHandReactionLogic,
                isAtStart: FastReactionAtStart,
                ResourceCost,
                PlayActionType.PlayFromHand);
        }

        /*******************************************************************/
        private async Task PlayFromHandReactionLogic(GameAction gameAction)
        {
            await PlayFromHandCommand.RunWith(ControlOwner);
        }

        protected override bool CanPlayFromHandWith(GameAction gameAction)
        {
            if (CurrentZone.ZoneType != ZoneType.Hand) return false;
            if (ControlOwner?.Resources.Value < ResourceCost.Value) return false;
            return CanPlayFromHandSpecific(gameAction);
        }

        /*******************************************************************/

        protected override abstract Task ExecuteConditionEffect(Investigator investigator);
    }
}
