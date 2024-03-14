using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{

    public class PlacePlotGameAction : GameAction
    {
        [Inject] private readonly GameActionProvider _gameActionProvider;
        [Inject] private readonly ChaptersProvider _chaptersProviders;

        public CardPlot CardPlot { get; }

        /*******************************************************************/
        public PlacePlotGameAction(CardPlot cardPlot)
        {
            CardPlot = cardPlot;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionProvider.Create(new UpdateStatesGameAction(CardPlot.FaceDown, false));
            await _gameActionProvider.Create(new MoveCardsGameAction(CardPlot, _chaptersProviders.CurrentScene.PlotZone));
            await _gameActionProvider.Create(new ShowHistoryGameAction(CardPlot.InitialHistory, CardPlot));
        }
    }
}
