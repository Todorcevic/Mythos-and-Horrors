using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public class RotateCardGameAction : GameAction
    {
        public Card Card { get; private set; }

        /*******************************************************************/
        public async Task Run(Card card)
        {
            Card = card;
            await Start();
        }

        protected override async Task ExecuteThisLogic()
        {
            Card.IsFaceDown = !Card.IsFaceDown;
            await Task.CompletedTask;
        }
    }
}
