using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class MoveCardsGameAction : GameAction
    {
        private readonly Dictionary<Card, Zone> _cardsWithPreviousZones;
        [Inject] private readonly IPresenter<MoveCardsGameAction> _moveCardPresenter;

        public IEnumerable<Card> Cards => _cardsWithPreviousZones.Keys;
        public Card Card => Cards.First();
        public Zone ToZone { get; }
        public bool IsSingleMove => Cards.Count() == 1;

        /*******************************************************************/
        public MoveCardsGameAction(IEnumerable<Card> cards, Zone zone)
        {
            ToZone = zone;
            _cardsWithPreviousZones = cards.ToDictionary(card => card, card => card.CurrentZone);
            CanBeExecuted = cards.Count() > 0;
        }

        public MoveCardsGameAction(Card card, Zone zone) : this(new[] { card }, zone) { }

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

        protected override async Task UndoThisLogic()
        {
            foreach (Card card in Cards)
            {
                ToZone.RemoveCard(card);
                _cardsWithPreviousZones[card].AddCard(card);
            }

            await _moveCardPresenter.PlayAnimationWith(this);
        }
    }
}

