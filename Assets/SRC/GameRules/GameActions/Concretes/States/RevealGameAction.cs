using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class RevealGameAction : GameAction
    {
        [Inject] private readonly IPresenter<RevealGameAction> _revealCardPresenter;
        [Inject] private readonly GameActionProvider _gameActionProvider;

        public IRevealable RevellableCard { get; }
        public Card Card => RevellableCard as Card;

        /*******************************************************************/
        public RevealGameAction(IRevealable cardReveled)
        {
            RevellableCard = cardReveled;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionProvider.Create(new UpdateStatesGameAction(RevellableCard.Revealed, true));
            await _revealCardPresenter.PlayAnimationWith(this);
            await RevellableCard.RevealEffect();
        }
    }
}
