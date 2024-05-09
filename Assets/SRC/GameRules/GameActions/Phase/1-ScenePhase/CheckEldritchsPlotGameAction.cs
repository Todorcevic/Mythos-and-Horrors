using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CheckEldritchsPlotGameAction : GameAction
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        public bool IsComplete => _chaptersProvider.CurrentScene.CurrentPlot.Eldritch.Value -
            (_cardsProvider.AllCards.OfType<IEldritchable>().Sum(eldrichable => eldrichable.Eldritch.Value)) <= 0;

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (!IsComplete) return;
            await _gameActionsProvider.Create(new RevealGameAction(_chaptersProvider.CurrentScene.CurrentPlot));
        }
    }
}
