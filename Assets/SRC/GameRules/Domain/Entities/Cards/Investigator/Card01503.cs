using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01503 : CardInvestigator, IActivable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public State AbilityUsed { get; private set; }
        public Stat ActivateTurnsCost { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            AbilityUsed = new State(false);
            ActivateTurnsCost = new Stat(0);
        }

        /*******************************************************************/
        protected override async Task WhenBegin(GameAction gameAction)
        {
            await base.WhenBegin(gameAction);
            if (gameAction is RoundGameAction) await _gameActionsProvider.Create(new UpdateStatesGameAction(AbilityUsed, false));
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
        public async Task Activate()
        {
            await _gameActionsProvider.Create(new PayResourceGameAction(Owner, 2));
            await _gameActionsProvider.Create(new IncrementStatGameAction(Owner.CurrentTurns, 1));
            await _gameActionsProvider.Create(new UpdateStatesGameAction(AbilityUsed, true));
        }

        public bool ConditionToActivate(Investigator investigator)
        {
            if (AbilityUsed.IsActive) return false;
            if (!IsInPlay) return false;
            if (Owner != investigator) return false;
            if (Owner.Resources.Value < 2) return false;
            return true;
        }
    }
}
