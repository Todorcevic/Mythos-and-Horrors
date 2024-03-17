using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class MoveCardsGameAction : GameAction
    {
        [Inject] private readonly IPresenter<MoveCardsGameAction> _moveCardPresenter;

        public IEnumerable<Card> Cards { get; }
        public Dictionary<Card, Zone> PreviousZones { get; } = new();
        public Card Card => Cards.First();
        public Zone ToZone { get; }
        public bool IsSingleMove => Cards.Count() == 1;

        /*******************************************************************/
        public MoveCardsGameAction(IEnumerable<Card> cards, Zone zone)
        {
            Cards = cards;
            ToZone = zone;
        }

        public MoveCardsGameAction(Card card, Zone zone) : this(new[] { card }, zone) { }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            foreach (Card card in Cards)
            {
                PreviousZones.Add(card, card.CurrentZone);
                card.CurrentZone?.RemoveCard(card);
                ToZone.AddCard(card);
            }

            await _moveCardPresenter.PlayAnimationWith(this);
        }

        protected override async Task UndoThisLogic()
        {
            foreach (Card card in Cards)
            {
                ToZone.RemoveCard(card);
                PreviousZones[card].AddCard(card);
            }

            await _moveCardPresenter.UndoAnimationWith(this);
        }
    }
}

