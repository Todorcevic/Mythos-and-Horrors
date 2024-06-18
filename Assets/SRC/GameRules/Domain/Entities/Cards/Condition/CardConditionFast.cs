using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardConditionFast : CardCondition
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;

        protected abstract GameActionTime FastReactionAtStart { get; }
        protected override bool IsFast => true;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateFastPlayCondition<GameAction>();
        }

        /*******************************************************************/
        private OptativeReaction<T> CreateFastPlayCondition<T>() where T : GameAction
        {
            Func<T, bool> condition = PlayFromHandCondition.IsTrueWith;
            Func<T, Task> logic = PlayFromHand;

            return CreateRealReaction(condition, logic, FastReactionAtStart, PlayFromHandActionType, isBase: true);
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
        protected override async Task PlayFromHand(GameAction gameAction)
        {
            Investigator currentInvestigator = ControlOwner;
            await _gameActionsProvider.Create(new MoveCardsGameAction(this, _chaptersProvider.CurrentScene.LimboZone));
            await ExecuteConditionEffect(gameAction, currentInvestigator);
            await _gameActionsProvider.Create(new DiscardGameAction(this));
        }

        protected abstract Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator);
    }
}
