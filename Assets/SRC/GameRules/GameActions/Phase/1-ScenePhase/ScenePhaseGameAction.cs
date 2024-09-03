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
        //1.1	Mythos phase begins.
        protected override async Task ExecuteThisPhaseLogic()
        {
            //1.2	Place 1 doom on the current agenda. (DecrementStatGameAction.Parent is ScenePhaseGameAction)
            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(_chaptersProvider.CurrentScene.CurrentPlot?.Eldritch, 1).Execute();
            //1.3	Check doom threshold.
            await _gameActionsProvider.Create<CheckEldritchsPlotGameAction>().Execute();
            //1.4	Each investigator draws 1 encounter card.
            await _gameActionsProvider.Create<InvestigatorsDrawDangerCardGameAction>().Execute();
        }
        //1.5	Mythos phase ends.
    }
}
