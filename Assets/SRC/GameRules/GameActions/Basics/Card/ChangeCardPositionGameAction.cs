using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class ChangeCardPositionGameAction : GameAction
    {
        private int lastPosition;

        public Card Card { get; private set; }
        public int NewPosition { get; private set; }

        /*******************************************************************/
        public ChangeCardPositionGameAction SetWith(Card card, int newPosition)
        {
            Card = card;
            NewPosition = newPosition;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            lastPosition = Card.CurrentZone.Cards.IndexOf(Card);
            Card.CurrentZone.ChangePositionOf(Card, NewPosition);
            await Task.CompletedTask;
        }

        /*******************************************************************/
        public override async Task Undo()
        {
            Card.CurrentZone.ChangePositionOf(Card, lastPosition);
            await base.Undo();
        }
    }
}

