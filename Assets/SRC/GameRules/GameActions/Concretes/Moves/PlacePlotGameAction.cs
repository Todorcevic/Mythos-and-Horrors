using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PlacePlotGameAction : GameAction
    {
        [Inject] private readonly ChaptersProvider _chaptersProviders;

        public CardPlot CardPlot { get; private set; }

        public override bool CanBeExecuted => CardPlot != null;

        /*******************************************************************/
        public PlacePlotGameAction SetWith(CardPlot cardPlot)
        {
            CardPlot = cardPlot;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(CardPlot.FaceDown, false).Execute();
            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(CardPlot, _chaptersProviders.CurrentScene.PlotZone).Execute();
            await _gameActionsProvider.Create<ShowHistoryGameAction>().SetWith(CardPlot.InitialHistory, CardPlot).Execute();
        }
    }
}
