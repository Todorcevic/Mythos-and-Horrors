using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardConditionTrigged : CardCondition
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        protected abstract bool FastReactionAtStart { get; }
        protected override bool IsFast => true;
        public Reaction<GameAction> PlayFromHandReaction { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            PlayFromHandReaction = CreateOptativeReaction<GameAction>(PlayFromHandCondition.IsTrueWith,
                PlayFromHand,
                isAtStart: FastReactionAtStart,
                ResourceCost,
                PlayFromHandActionType);
        }

        /*******************************************************************/
        protected sealed override bool CanPlayFromHandWith(GameAction gameAction)
        {
            if (CurrentZone.ZoneType != ZoneType.Hand) return false;
            if (ControlOwner?.Resources.Value < ResourceCost.Value) return false;
            return CanPlayFromHandSpecific(gameAction);
        }

        protected abstract bool CanPlayFromHandSpecific(GameAction gameAction);

        /*******************************************************************/
        public override async Task PlayFromHand(GameAction gameAction)
        {
            Investigator currentInvestigator = ControlOwner;
            await _gameActionsProvider.Create(new MoveCardsGameAction(this, _chaptersProvider.CurrentScene.LimboZone));
            await ExecuteConditionEffect(gameAction, currentInvestigator);
            await _gameActionsProvider.Create(new DiscardGameAction(this));
        }

        protected abstract Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator);
    }
}
