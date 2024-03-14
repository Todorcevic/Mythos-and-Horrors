using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CheckEldritchPlotGameAction : GameAction
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly GameActionProvider _gameActionFactory;

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (!_chaptersProvider.CurrentScene.CurrentPlot.IsComplete) return;
            if (_chaptersProvider.CurrentScene.CurrentPlot.Revealed.IsActive) return;

            await _gameActionFactory.Create(new RevealGameAction(_chaptersProvider.CurrentScene.CurrentPlot));
        }
    }
}
