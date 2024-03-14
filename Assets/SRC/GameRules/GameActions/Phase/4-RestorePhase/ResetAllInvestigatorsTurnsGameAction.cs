using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ResetAllInvestigatorsTurnsGameAction : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly GameActionProvider _gameActionProvider;

        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(ResetAllInvestigatorsTurnsGameAction);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(ResetAllInvestigatorsTurnsGameAction);
        public override Phase MainPhase => Phase.Restore;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            Dictionary<Stat, int> turnsInvestigastors = _investigatorsProvider.AllInvestigators
                .ToDictionary(investigator => investigator.Turns, investigator => investigator.Turns.MaxValue);
            await _gameActionProvider.Create(new UpdateStatGameAction(turnsInvestigastors));
        }
    }
}
