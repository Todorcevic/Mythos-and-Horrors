using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class RotateCardGameAction : GameAction
    {
        [Inject] private readonly IPresenter<RotateCardGameAction> _rotateCardPresenter;

        public Card Card { get; }
        public bool ToFaceDown { get; }

        /*******************************************************************/
        public RotateCardGameAction(Card card, bool toFaceDown)
        {
            Card = card;
            ToFaceDown = toFaceDown;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (Card.IsFaceDown == ToFaceDown) return;
            Card.TurnDown(ToFaceDown);
            await _rotateCardPresenter.PlayAnimationWith(this);
        }
    }
}

