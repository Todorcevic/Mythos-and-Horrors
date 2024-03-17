using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CheckEldritchsPlotGameAction : GameAction
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (!_chaptersProvider.CurrentScene.CurrentPlot.IsComplete) return;
            await _gameActionsProvider.Create(new RevealGameAction(_chaptersProvider.CurrentScene.CurrentPlot));
        }
    }
}
