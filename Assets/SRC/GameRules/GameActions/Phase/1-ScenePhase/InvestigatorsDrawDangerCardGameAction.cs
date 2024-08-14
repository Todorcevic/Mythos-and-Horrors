using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class InvestigatorsDrawDangerCardGameAction : PhaseGameAction
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        /*******************************************************************/
        public override string Name => _textsProvider.GetLocalizableText("PhaseName_InvestigatorsDrawDangerCard");
        public override string Description => _textsProvider.GetLocalizableText("PhaseDescription_InvestigatorsDrawDangerCard");
        public override Phase MainPhase => Phase.Scene;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create<SafeForeach<Investigator>>().SetWith(AllInvestigatorsInPlay, DrawDangerCard).Execute();

            /*******************************************************************/
            IEnumerable<Investigator> AllInvestigatorsInPlay() => _investigatorsProvider.AllInvestigatorsInPlay;
            async Task DrawDangerCard(Investigator investigator) =>
                     await _gameActionsProvider.Create<DrawDangerGameAction>().SetWith(investigator).Execute();
        }
    }
}
