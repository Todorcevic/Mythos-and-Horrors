using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardConditionFast : CardCondition
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        protected abstract bool FastReactionAtStart { get; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            RemoveStat(PlayFromHandTurnsCost);
            PlayFromHandTurnsCost = CreateStat(0);
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
            return CanPlayFromHandOverride(gameAction);
        }
        protected abstract bool CanPlayFromHandOverride(GameAction gameAction);

        /*******************************************************************/

        protected override abstract Task ExecuteConditionEffect(Investigator investigator);
    }
}
