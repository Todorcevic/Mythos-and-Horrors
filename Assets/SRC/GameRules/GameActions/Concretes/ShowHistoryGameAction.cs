using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class ShowHistoryGameAction : GameAction
    {
        [Inject] private readonly IShowHistoryAnimator _showHistory;
        public History History { get; private set; }

        /*******************************************************************/
        public async Task Run(History history)
        {
            History = history;
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _showHistory.ShowHistory(this);
        }
    }
}
