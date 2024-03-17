using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class AllInvestigatorsDrawCardAndResource : PhaseGameAction
    {
        private readonly List<DrawAidGameAction> _drawAidGameActions = new();
        private readonly List<GainResourceGameAction> _gainResourceGameActions = new();
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(AllInvestigatorsDrawCardAndResource);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(AllInvestigatorsDrawCardAndResource);
        public override Phase MainPhase => Phase.Restore;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            foreach (Investigator investigator in _investigatorsProvider.AllInvestigatorsInPlay)
            {
                _drawAidGameActions.Add(await _gameActionsProvider.Create(new DrawAidGameAction(investigator)));
                _gainResourceGameActions.Add(await _gameActionsProvider.Create(new GainResourceGameAction(investigator, 1)));
            }
        }

        protected override async Task UndoThisPhaseLogic()
        {
            while (_drawAidGameActions.Any() || _gainResourceGameActions.Any())
            {
                if (_gainResourceGameActions.Any())
                {
                    await _gainResourceGameActions.Last().Undo();
                    _gainResourceGameActions.RemoveAt(_gainResourceGameActions.Count - 1);
                }
                if (_drawAidGameActions.Any())
                {
                    await _drawAidGameActions.Last().Undo();
                    _drawAidGameActions.RemoveAt(_drawAidGameActions.Count - 1);
                }
            }
        }
    }
}
