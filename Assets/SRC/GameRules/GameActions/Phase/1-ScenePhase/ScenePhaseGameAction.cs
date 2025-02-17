using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ScenePhaseGameAction : PhaseGameAction, IPhase
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public override Phase MainPhase => Phase.Scene;
        public override Localization PhaseNameLocalization => new("PhaseName_ScenePhase");
        public override Localization PhaseDescriptionLocalization => new("PhaseDescription_ScenePhase");

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(_chaptersProvider.CurrentScene.CurrentPlot?.Eldritch, 1).Execute();
            await _gameActionsProvider.Create<CheckEldritchsPlotGameAction>().Execute();
            await _gameActionsProvider.Create<InvestigatorsDrawDangerCardGameAction>().Execute();
        }
    }
}
