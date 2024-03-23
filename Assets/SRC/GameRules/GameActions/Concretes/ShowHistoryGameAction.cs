using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ShowHistoryGameAction : GameAction
    {
        [Inject] private readonly IPresenter<ShowHistoryGameAction> _showHistoryPresenter;

        public History History { get; }
        public Card Card { get; }
        public override bool CanUndo => false;

        /*******************************************************************/
        public ShowHistoryGameAction(History history, Card card = null)
        {
            History = history;
            Card = card;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _showHistoryPresenter.PlayAnimationWith(this);
        }
    }
}
