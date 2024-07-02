using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01128 : CardPlace
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public Dictionary<Investigator, State> InvestigatorsUsed { get; } = new();
        public override IEnumerable<Tag> Tags => new[] { Tag.Arkham };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            _investigatorsProvider.AllInvestigators.ForEach(investigator => InvestigatorsUsed.Add(investigator, CreateState(false)));
            CreateActivation(1, HealthFearLogic, HealthFearCondition, PlayActionType.Activate);
        }

        /*******************************************************************/
        private async Task HealthFearLogic(Investigator investigator)
        {
            await _gameActionsProvider.Create<HealthGameAction>().SetWith(investigator.InvestigatorCard, amountDamageToRecovery: 3).Start();
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(InvestigatorsUsed[investigator], true).Start();
        }

        private bool HealthFearCondition(Investigator investigator)
        {
            if (investigator.CurrentPlace != this) return false;
            if (InvestigatorsUsed[investigator].IsActive) return false;
            if (investigator.DamageRecived.Value <= 0) return false;
            return true;
        }
    }
}
