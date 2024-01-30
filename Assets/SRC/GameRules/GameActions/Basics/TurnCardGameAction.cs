using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class TurnCardGameAction : GameAction
    {
        [Inject] private readonly IViewLayer _viewLayer;

        public Card Card { get; }
        public bool ToFaceDown { get; }

        /*******************************************************************/
        public TurnCardGameAction(Card card, bool toFaceDown)
        {
            Card = card;
            ToFaceDown = toFaceDown;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Card.TurnDown(ToFaceDown);
            await _viewLayer.PlayAnimationWith(this);
        }

    }
}

