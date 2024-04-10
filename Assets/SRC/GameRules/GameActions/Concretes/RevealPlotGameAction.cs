using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class RevealPlotGameAction : GameAction
    {
        public CardPlot CardPlot { get; init; }

        /*******************************************************************/
        public RevealPlotGameAction(CardPlot cardPlot)
        {
            CardPlot = cardPlot;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await CardPlot.RevealEffect();
        }
    }
}
