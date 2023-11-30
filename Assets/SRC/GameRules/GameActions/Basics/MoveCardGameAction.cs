using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class MoveCardGameAction : GameAction
    {
        [Inject] private readonly ICardMover _cardMover;

        public Card Card { get; private set; }
        public Zone Zone { get; private set; }

        /*******************************************************************/
        public async Task Run(Card card, Zone zone)
        {
            Card = card;
            Zone = zone;
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Card.CurrentZone?.RemoveCard(Card);
            Card.MoveToZone(Zone);
            Zone.AddCard(Card);

            await _cardMover.MoveCardToZoneAsync(Card, Zone);
        }
    }
}

