using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.EditMode
{
    public class RotateCardGameAction : GameAction
    {
        [Inject] private readonly ICardRotator _cardRorator;
        private bool isAsync;

        public Card Card { get; private set; }

        /*******************************************************************/
        public async Task Run(Card card, bool isAsync = true)
        {
            Card = card;
            this.isAsync = isAsync;
            await Start();
        }

        protected override async Task ExecuteThisLogic()
        {
            Card.IsFaceDown = !Card.IsFaceDown;
            if (isAsync) await _cardRorator.RotateAsync(Card);
            else _cardRorator.Rotate(Card);
        }
    }
}
