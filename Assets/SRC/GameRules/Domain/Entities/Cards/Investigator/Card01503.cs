using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01503 : CardInvestigator
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public State AbilityUsed { get; private set; }
        public IReaction RestartAbilityReaction { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            AbilityUsed = new State(false);
            CreateActivation(CreateStat(0), GainTurnActivate, GainTurnConditionToActivate);
            RestartAbilityReaction = CreateBeginReaction<RoundGameAction>(RestartAbilityCondition, RestartAbilityLogic);
        }

        /*******************************************************************/
        public override async Task StarEffect()
        {
            _gameActionsProvider.CurrentChallenge.SuccesEffects.Add(DrawResources);
            await Task.CompletedTask;
        }

        public override int StarValue() => 2;

        private async Task DrawResources()
        {
            await _gameActionsProvider.Create(new GainResourceGameAction(Owner, 2));
        }

        /*******************************************************************/
        public async Task GainTurnActivate(Investigator activeInvestigator)
        {
            await _gameActionsProvider.Create(new PayResourceGameAction(activeInvestigator, 2));
            await _gameActionsProvider.Create(new IncrementStatGameAction(activeInvestigator.CurrentTurns, 1));
            await _gameActionsProvider.Create(new UpdateStatesGameAction(AbilityUsed, true));
        }

        public bool GainTurnConditionToActivate(Investigator activeInvestigator)
        {
            if (AbilityUsed.IsActive) return false;
            if (!IsInPlay) return false;
            if (Owner != activeInvestigator) return false;
            if (activeInvestigator.Resources.Value < 2) return false;
            return true;
        }

        /*******************************************************************/
        private async Task RestartAbilityLogic(RoundGameAction roudnGameAction)
        {
            await _gameActionsProvider.Create(new UpdateStatesGameAction(AbilityUsed, false));
        }

        private bool RestartAbilityCondition(RoundGameAction roudnGameAction)
        {
            if (!AbilityUsed.IsActive) return false;
            return true;
        }
    }
}
