using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01501 : CardInvestigator
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public State AbilityUsed { get; private set; }
        public override IEnumerable<Tag> Tags => new[] { Tag.Agency, Tag.Detective };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Zenject injects this method")]
        private void Init()
        {
            AbilityUsed = CreateState(false);
            CreateOptativeReaction<DefeatCardGameAction>(DiscoverHintCondition, DiscoverHintLogic, GameActionTime.After);
            CreateForceReaction<RoundGameAction>(RestartAbilityCondition, RestartAbilityLogic, GameActionTime.Before);
        }

        /*******************************************************************/
        protected override async Task StarEffect() => await Task.CompletedTask;

        protected override int StarValue() => Owner.CurrentPlace.Hints.Value;

        /*******************************************************************/
        private async Task DiscoverHintLogic(DefeatCardGameAction defeatCardGameAction)
        {
            await _gameActionsProvider.Create(new GainHintGameAction(Owner, Owner.CurrentPlace.Hints, 1));
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(AbilityUsed, true).Start();
        }

        private bool DiscoverHintCondition(DefeatCardGameAction defeatCardGameAction)
        {
            if (defeatCardGameAction.ByThisInvestigator != Owner) return false;
            if (defeatCardGameAction.Card is not CardCreature) return false;
            if (!IsInPlay) return false;
            if (Owner.CurrentPlace.Hints.Value < 1) return false;
            if (AbilityUsed.IsActive) return false;
            return true;
        }

        /*******************************************************************/
        private async Task RestartAbilityLogic(RoundGameAction roundGameAction)
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(AbilityUsed, false).Start();
        }

        private bool RestartAbilityCondition(RoundGameAction roundGameAction)
        {
            if (!AbilityUsed.IsActive) return false;
            return true;
        }
    }
}
