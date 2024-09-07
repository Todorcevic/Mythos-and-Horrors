using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01503 : CardInvestigator
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public State AbilityUsed { get; private set; }
        public override IEnumerable<Tag> Tags => new[] { Tag.Criminal };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            AbilityUsed = CreateState(false);
            CreateFastActivation(GainTurnActivate, GainTurnConditionToActivate, PlayActionType.Activate, new Localization("Activation_Card01503"));
            CreateForceReaction<RoundGameAction>(RestartAbilityCondition, RestartAbilityLogic, GameActionTime.Before);
        }

        /*******************************************************************/
        protected override async Task StarEffect()
        {
            _gameActionsProvider.CurrentChallenge.SuccesEffects.Add(DrawResources);
            await Task.CompletedTask;
        }

        protected override int StarValue() => 2;

        private async Task DrawResources()
        {
            await _gameActionsProvider.Create<GainResourceGameAction>().SetWith(Owner, 2).Execute();
        }

        /*******************************************************************/
        public async Task GainTurnActivate(Investigator activeInvestigator)
        {
            await _gameActionsProvider.Create<PayResourceGameAction>().SetWith(activeInvestigator, 2).Execute();
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(activeInvestigator.CurrentActions, 1).Execute();
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(AbilityUsed, true).Execute();
        }

        public bool GainTurnConditionToActivate(Investigator activeInvestigator)
        {
            if (AbilityUsed.IsActive) return false;
            if (IsInPlay.IsFalse) return false;
            if (Owner != activeInvestigator) return false;
            if (activeInvestigator.Resources.Value < 2) return false;
            return true;
        }

        /*******************************************************************/
        private async Task RestartAbilityLogic(RoundGameAction roudnGameAction)
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(AbilityUsed, false).Execute();
        }

        private bool RestartAbilityCondition(RoundGameAction roudnGameAction)
        {
            if (!AbilityUsed.IsActive) return false;
            return true;
        }
    }
}
