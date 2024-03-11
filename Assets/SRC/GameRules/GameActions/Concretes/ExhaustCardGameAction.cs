using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{

    public class ExhaustCardGameAction : GameAction
    {
        [Inject] private readonly IPresenter<ExhaustCardGameAction> _exhaustCardPresenter;

        public Card Card { get; private set; }

        /*******************************************************************/
        public ExhaustCardGameAction(Card card)
        {
            Card = card;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Card.Exausted.UpdateValueTo(true);
            await _exhaustCardPresenter.PlayAnimationWith(this);
        }
    }
}