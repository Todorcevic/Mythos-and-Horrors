using System;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.EditMode
{
    public class MoveCardGameAction : GameAction
    {
        [Inject] private readonly ICardMover _cardMover;
        private bool isAsync;

        public Card Card { get; private set; }
        public Zone Zone { get; private set; }

        /*******************************************************************/
        public async Task Run(Card card, Zone zone, bool isAsync = true)
        {
            Card = card;
            Zone = zone;
            this.isAsync = isAsync;
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

