using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class ShowHistoryGameAction : GameAction
    {
        public History History { get; private set; }
        public Card Card { get; private set; } // TODO: la vista puede conseguir esta información buscando en CardInfo
        public override bool CanUndo => false;
        public override bool CanBeExecuted => History != null;

        /*******************************************************************/
        public ShowHistoryGameAction SetWith(History history, Card card = null)
        {
            History = history;
            Card = card;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await Task.CompletedTask;
        }
    }
}
