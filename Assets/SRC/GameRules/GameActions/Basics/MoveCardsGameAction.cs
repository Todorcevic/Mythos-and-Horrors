using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class MoveCardsGameAction : GameAction
    {
        [Inject] private readonly ViewLayersProvider _viewLayerProvider;

        public List<Card> Cards { get; }
        public Card Card => Cards[0];
        public Zone Zone { get; }
        public bool IsSingleMove => Cards.Count == 1;

        /*******************************************************************/
        public MoveCardsGameAction(List<Card> cards, Zone zone)
        {
            Cards = cards;
            Zone = zone;
        }

        public MoveCardsGameAction(Card card, Zone zone)
        {
            Cards = new List<Card> { card };
            Zone = zone;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            foreach (Card card in Cards)
            {
                card.CurrentZone?.RemoveCard(card);
                Zone.AddCard(card);
            }

            await _viewLayerProvider.PlayAnimationWith(this);
        }
    }
}

