using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ReadyCardGameAction : GameAction
    {
        [Inject] private readonly IPresenter<ReadyCardGameAction> _readyCardPresenter;

        public Card Card { get; private set; }

        /*******************************************************************/
        public ReadyCardGameAction(Card card)
        {
            Card = card;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Card.Exausted.UpdateValueTo(false);
            await _readyCardPresenter.PlayAnimationWith(this);
        }
    }
}