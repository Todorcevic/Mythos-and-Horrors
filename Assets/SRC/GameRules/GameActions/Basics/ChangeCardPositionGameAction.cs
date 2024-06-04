using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ChangeCardPositionGameAction : GameAction
    {
        [Inject] private readonly IPresenter<ChangeCardPositionGameAction> _chagePositionPresenter;
        private int lastPosition;

        public Card Card { get; }
        public int NewPosition { get; }

        /*******************************************************************/
        public ChangeCardPositionGameAction(Card card, int newPosition)
        {
            Card = card;
            NewPosition = newPosition;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            lastPosition = Card.CurrentZone.Cards.IndexOf(Card);
            Card.CurrentZone.ChangePositionOf(Card, NewPosition);
            await _chagePositionPresenter.PlayAnimationWith(this);
        }

        /*******************************************************************/
        public override async Task Undo()
        {
            Card.CurrentZone.ChangePositionOf(Card, lastPosition);
            await base.Undo();
            await _chagePositionPresenter.PlayAnimationWith(this);
        }
    }
}

