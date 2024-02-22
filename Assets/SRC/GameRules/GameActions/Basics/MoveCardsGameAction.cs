using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class MoveCardsGameAction : GameAction
    {
        [Inject] private readonly IPresenter<MoveCardsGameAction> _moveCardPresenter;

        public List<Card> Cards { get; }
        public Card Card => Cards[0];
        public Zone ToZone { get; }
        public Zone FromZone { get; }
        public bool IsSingleMove => Cards.Count == 1;

        /*******************************************************************/
        public MoveCardsGameAction(List<Card> cards, Zone zone)
        {
            Cards = cards;
            ToZone = zone;
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

            await _moveCardPresenter.PlayAnimationWith(this);
        }
    }
}

