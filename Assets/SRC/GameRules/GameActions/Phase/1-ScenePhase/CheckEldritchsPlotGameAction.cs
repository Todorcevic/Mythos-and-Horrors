using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CheckEldritchsPlotGameAction : GameAction
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public bool IsComplete => _chaptersProvider.CurrentScene.CurrentPlot.AmountOfEldritch <= 0;

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (!IsComplete) return;
            await _gameActionsProvider.Create<RevealGameAction>().SetWith(_chaptersProvider.CurrentScene.CurrentPlot).Execute();
        }
    }
}
