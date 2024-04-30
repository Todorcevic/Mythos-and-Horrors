using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01501 : CardInvestigator
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public State AbilityUsed { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Zenject injects this method")]
        private void Init()
        {
            AbilityUsed = new State(false);
        }

        /*******************************************************************/
        public override async Task StarEffect() => await Task.CompletedTask;

        public override int StarValue() => Owner.CurrentPlace.Hints.Value;

        /*******************************************************************/
        protected override async Task WhenFinish(GameAction gameAction)
        {
            await base.WhenFinish(gameAction);
            await OptativeReaction<DefeatCardGameAction>(gameAction, DiscoverHintCondition, DiscoverHintLogic);
        }

        protected override async Task WhenBegin(GameAction gameAction)
        {
            await base.WhenBegin(gameAction);
            if (gameAction is RoundGameAction) await _gameActionsProvider.Create(new UpdateStatesGameAction(AbilityUsed, false));
        }

        /*******************************************************************/
        private async Task DiscoverHintLogic(DefeatCardGameAction defeatCardGameAction)
        {
            await _gameActionsProvider.Create(new GainHintGameAction(Owner, Owner.CurrentPlace.Hints, 1));
            await _gameActionsProvider.Create(new UpdateStatesGameAction(AbilityUsed, true));
        }

        private bool DiscoverHintCondition(DefeatCardGameAction defeatCardGameAction)
        {
            if (defeatCardGameAction.ByThisInvestigator != Owner) return false;
            if (!IsInPlay) return false;
            if (Owner.CurrentPlace.Hints.Value < 1) return false;
            if (AbilityUsed.IsActive) return false;
            return true;
        }
    }
}
