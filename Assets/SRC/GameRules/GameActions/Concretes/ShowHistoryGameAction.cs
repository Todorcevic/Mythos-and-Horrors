using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ShowHistoryGameAction : GameAction
    {
        [Inject] private readonly IPresenter<ShowHistoryGameAction> _showHistoryPresenter;

        public History History { get; private set; }
        public Card Card { get; private set; }
        public override bool CanUndo => false;

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
            await _showHistoryPresenter.PlayAnimationWith(this);
        }
    }
}
