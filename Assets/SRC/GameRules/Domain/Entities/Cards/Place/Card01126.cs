using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01126 : CardPlace
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
            CreateActivation(1, DrawCardsLogic, DrawCardsCondition, PlayActionType.Activate);
        }

        /*******************************************************************/
        private bool DrawCardsCondition(Investigator investigator)
        {
            if (investigator.CurrentPlace != this) return false;
            if (InvestigatorsUsed[investigator].IsActive) return false;
            return true;
        }

        private async Task DrawCardsLogic(Investigator investigator)
        {
            await _gameActionsProvider.Create(new DrawAidGameAction(investigator));
            await _gameActionsProvider.Create(new DrawAidGameAction(investigator));
            await _gameActionsProvider.Create(new DrawAidGameAction(investigator));
            await _gameActionsProvider.Create(new UpdateStatesGameAction(InvestigatorsUsed[investigator], true));
        }
    }
}
