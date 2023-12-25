using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class ShowHistoryGameAction : GameAction
    {
        private History _history;
        [Inject] private readonly IHistoryShower _historyShower;

        /*******************************************************************/
        public async Task Run(History history)
        {
            _history = history;
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _historyShower.ShowHistory(_history);
        }
    }
}
