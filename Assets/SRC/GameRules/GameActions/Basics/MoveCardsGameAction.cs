using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class MoveCardsGameAction : GameAction
    {
        [Inject] private readonly ICardMover _cardMover;
        [Inject] private readonly IAdventurerSelector _adventurerSelector;

        public List<Card> Cards { get; private set; }
        public Zone Zone { get; private set; }
        public bool IsSingleMove => Cards.Count == 1;

        /*******************************************************************/
        private async Task Run(Zone zone, params Card[] cards)
        {
            Cards = cards.ToList();
            Zone = zone;
            await Start();
        }

        public async Task Run(List<Card> cards, Zone zone) => await Run(zone, cards.ToArray());
        public async Task Run(Card card, Zone zone) => await Run(zone, card);


        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            foreach (Card card in Cards)
            {
                card.CurrentZone?.RemoveCard(card);
                card.MoveToZone(Zone);
                Zone.AddCard(card);
            }

            await Animation();
        }

        private async Task Animation()
        {
            await _adventurerSelector.Select(Zone);
            if (IsSingleMove) await _cardMover.MoveCardToZone(Cards[0], Zone);
            else await _cardMover.MoveCardsToZone(Cards, Zone);
        }
    }
}

