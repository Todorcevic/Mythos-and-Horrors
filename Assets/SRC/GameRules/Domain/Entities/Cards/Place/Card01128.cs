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
            CreateActivation(1, HealthLogic, HealthCondition, PlayActionType.Activate, new Localization("Activation_Card01128"));
        }

        /*******************************************************************/
        private async Task HealthLogic(Investigator investigator)
        {
            await _gameActionsProvider.Create<RecoverGameAction>().SetWith(investigator.InvestigatorCard, amountDamageToRecovery: 3).Execute();
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(InvestigatorsUsed[investigator], true).Execute();
        }

        private bool HealthCondition(Investigator investigator)
        {
            if (investigator.CurrentPlace != this) return false;
            if (InvestigatorsUsed[investigator].IsActive) return false;
            if (investigator.DamageRecived.Value <= 0) return false;
            return true;
        }
    }
}
