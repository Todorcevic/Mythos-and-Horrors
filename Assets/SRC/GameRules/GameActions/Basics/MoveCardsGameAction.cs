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
        public Zone ToZone { get; }
        public Zone FromZone { get; }
        public bool IsSingleMove => Cards.Count == 1;
        public List<Card> CardsInOutHand { get; }

        /*******************************************************************/
        public MoveCardsGameAction(List<Card> cards, Zone zone)
        {
            Cards = cards;
            ToZone = zone;

            if (ToZone.IsHandZone) CardsInOutHand = Cards;
            else CardsInOutHand = Cards.FindAll(card => card.IsInHand);
        }

        public MoveCardsGameAction(Card card, Zone zone) : this(new List<Card> { card }, zone) { }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            foreach (Card card in Cards)
            {
                card.CurrentZone?.RemoveCard(card);
                ToZone.AddCard(card);
            }

            await _viewLayerProvider.PlayAnimationWith(this);
        }
    }
}

