using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ScenePhaseGameAction : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public override Phase MainPhase => Phase.Scene;
        public override string Name => _textsProvider.GameText.SCENE_PHASE_NAME;
        public override string Description => _textsProvider.GameText.SCENE_PHASE_DESCRIPTION;

        /*******************************************************************/
        //1.1	Mythos phase begins.
        protected override async Task ExecuteThisPhaseLogic()
        {
            //1.2	Place 1 doom on the current agenda. (DecrementStatGameAction.Parent is ScenePhaseGameAction)
            await _gameActionsProvider.Create(new DecrementStatGameAction(_chaptersProvider.CurrentScene.CurrentPlot?.Eldritch, 1));
            //1.3	Check doom threshold.
            await _gameActionsProvider.Create(new CheckEldritchsPlotGameAction());
            //1.4	Each investigator draws 1 encounter card.
            await _gameActionsProvider.Create(new InvestigatorsDrawDangerCard());
        }
        //1.5	Mythos phase ends.
    }
}
