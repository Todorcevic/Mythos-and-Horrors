using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    //1.1	Mythos phase begins.
    public class ScenePhaseGameAction : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionProvider _gameActionFactory;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public override Phase MainPhase => Phase.Scene;
        public override string Name => _textsProvider.GameText.SCENE_PHASE_NAME;
        public override string Description => _textsProvider.GameText.SCENE_PHASE_DESCRIPTION;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionFactory.Create(new IncrementStatGameAction(_chaptersProvider.CurrentScene.CurrentPlot.Eldritch, 1));

        }
    }
    //1.5	Mythos phase ends.
}
