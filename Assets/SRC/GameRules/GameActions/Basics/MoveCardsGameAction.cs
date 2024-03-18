using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class MoveCardsGameAction : GameAction
    {
        private readonly IEnumerable<Card> _cards;
        private Dictionary<Card, Zone> _cardsWithPreviousZones;
        [Inject] private readonly IPresenter<MoveCardsGameAction> _moveCardPresenter;

        public bool IsUndo { get; private set; }
        public IEnumerable<Card> Cards => _cards.ToList();
        public Card Card => Cards.First();
        public Zone ToZone { get; }
        public bool IsSingleMove => Cards.Count() == 1;

        /*******************************************************************/
        public MoveCardsGameAction(IEnumerable<Card> cards, Zone zone)
        {
            ToZone = zone;
            _cards = cards;
            CanBeExecuted = cards.Count() > 0;
        }

        public MoveCardsGameAction(Card card, Zone zone) : this(new[] { card }, zone) { }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            _cardsWithPreviousZones = _cards.ToDictionary(card => card, card => card.CurrentZone);
            foreach (Card card in Cards)
            {
                card.CurrentZone?.RemoveCard(card);
                ToZone.AddCard(card);
            }

            await _moveCardPresenter.PlayAnimationWith(this);
        }

        public override async Task Undo()
        {
            IsUndo = true;
            foreach (Card card in Cards)
            {
                ToZone.RemoveCard(card);
                _cardsWithPreviousZones[card].AddCard(card);
            }

            await _moveCardPresenter.PlayAnimationWith(this);
        }
    }
}

