using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class RevealGameAction : GameAction
    {
        [Inject] private readonly IPresenter<RevealGameAction> _revealCardPresenter;
        [Inject] private readonly GameActionProvider _gameActionFactory;

        public IRevellable RevellableCard { get; }
        public Card Card => RevellableCard as Card;

        /*******************************************************************/
        public RevealGameAction(IRevellable cardReveled)
        {
            RevellableCard = cardReveled;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            RevellableCard.Revealed.UpdateValueTo(true);
            await _revealCardPresenter.PlayAnimationWith(this);
            await _gameActionFactory.Create(new ShowHistoryGameAction(RevellableCard.RevealHistory, Card));
        }
    }
}
