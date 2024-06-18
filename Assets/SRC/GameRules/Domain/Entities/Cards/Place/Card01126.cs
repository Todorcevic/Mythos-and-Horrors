using System;
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
        private Dictionary<Investigator, State> _investigatorsDraws = new();

        public override IEnumerable<Tag> Tags => new[] { Tag.Arkham };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            _investigatorsProvider.AllInvestigators.ForEach(investigator => _investigatorsDraws.Add(investigator, CreateState(false)));
            CreateActivation(CreateStat(1), DrawCardsLogic, DrawCardsCondition, PlayActionType.Activate); ;
        }

        /*******************************************************************/
        private bool DrawCardsCondition(Investigator investigator)
        {
            if (investigator.CurrentPlace != this) return false;
            if (_investigatorsDraws[investigator].IsActive) return false;
            return true;
        }

        private async Task DrawCardsLogic(Investigator investigator)
        {
            await _gameActionsProvider.Create(new DrawAidGameAction(investigator));
            await _gameActionsProvider.Create(new DrawAidGameAction(investigator));
            await _gameActionsProvider.Create(new DrawAidGameAction(investigator));
            await _gameActionsProvider.Create(new UpdateStatesGameAction(_investigatorsDraws[investigator], true));
        }
    }
}
