using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardConditionTrigged : CardCondition
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        protected abstract bool FastReactionAtStart { get; }
        protected override bool IsFast => true;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateReaction<GameAction>(PlayFromHandCondition.IsTrueWith, PlayFromHandReactionLogic, isAtStart: FastReactionAtStart, isOptative: true);
        }

        /*******************************************************************/
        private async Task PlayFromHandReactionLogic(GameAction gameAction)
        {
            await _gameActionsProvider.Create(new PlayFromHandGameAction(this, ControlOwner));
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
