using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public class MoveCardsGameAction : GameAction
    {
        public List<Card> Cards { get; private set; }
        public Card Card => Cards[0];
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

            await Task.CompletedTask;
        }
    }
}

