using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class RotateCardGameAction : GameAction
    {
        [Inject] private readonly ICardRotator _cardRorator;

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
            await _cardRorator.Rotate(Card);
        }
    }
}
