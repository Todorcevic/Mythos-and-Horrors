using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ResetAllInvestigatorsTurnsGameAction : PhaseGameAction
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public override string Name => _textsProvider.GetLocalizableText("PhaseName_ResetAllInvestigatorsTurns");
        public override string Description => _textsProvider.GetLocalizableText("PhaseDescription_ResetAllInvestigatorsTurns");
        public override Phase MainPhase => Phase.Restore;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            Dictionary<Stat, int> turnsInvestigastors = _investigatorsProvider.AllInvestigatorsInPlay
                .ToDictionary(investigator => investigator.CurrentTurns, investigator => investigator.MaxTurns.Value);
            await _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(turnsInvestigastors).Execute();
        }
    }
}
