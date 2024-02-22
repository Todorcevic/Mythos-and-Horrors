using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class RevealGameAction : GameAction
    {
        [Inject] private readonly INewPresenter<RevealGameAction> _revealCardPresenter;
        [Inject] private readonly GameActionFactory _gameActionFactory;

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
            RevellableCard.Revealed.UpdateValue(true);
            await _revealCardPresenter.PlayAnimationWith(this);
            await _gameActionFactory.Create(new ShowHistoryGameAction(RevellableCard.RevealHistory, Card));
        }
    }
}
