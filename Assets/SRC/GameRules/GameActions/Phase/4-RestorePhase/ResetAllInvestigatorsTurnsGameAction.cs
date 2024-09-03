using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ResetAllInvestigatorsTurnsGameAction : PhaseGameAction
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public override Phase MainPhase => Phase.Restore;
        public override Localization PhaseNameLocalization => new("PhaseName_ResetAllInvestigatorsTurns");
        public override Localization PhaseDescriptionLocalization => new("PhaseDescription_ResetAllInvestigatorsTurns");

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            Dictionary<Stat, int> turnsInvestigastors = _investigatorsProvider.AllInvestigatorsInPlay
                .ToDictionary(investigator => investigator.CurrentTurns, investigator => investigator.MaxTurns.Value);
            await _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(turnsInvestigastors).Execute();
        }
    }
}
