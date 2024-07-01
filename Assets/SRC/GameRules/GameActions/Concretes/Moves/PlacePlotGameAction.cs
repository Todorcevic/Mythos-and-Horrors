using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PlacePlotGameAction : GameAction
    {
        [Inject] private readonly ChaptersProvider _chaptersProviders;

        public CardPlot CardPlot { get; }

        public override bool CanBeExecuted => CardPlot != null;

        /*******************************************************************/
        public PlacePlotGameAction(CardPlot cardPlot)
        {
            CardPlot = cardPlot;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new UpdateStatesGameAction(CardPlot.FaceDown, false));
            await _gameActionsProvider.Create(new MoveCardsGameAction(CardPlot, _chaptersProviders.CurrentScene.PlotZone));
            await _gameActionsProvider.Create(new ShowHistoryGameAction(CardPlot.InitialHistory, CardPlot));
        }
    }
}
