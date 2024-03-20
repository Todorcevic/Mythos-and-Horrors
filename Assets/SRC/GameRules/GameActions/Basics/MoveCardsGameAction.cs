using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class MoveCardsGameAction : GameAction
    {
        private readonly bool _isFaceDown;
        private readonly IEnumerable<Card> _cards;
        private Dictionary<Card, (Zone zone, bool faceDown)> _cardsWithUndoState;
        [Inject] private readonly IPresenter<MoveCardsGameAction> _moveCardPresenter;

        public IEnumerable<Card> Cards => _cards.ToList();
        public Card Card => Cards.First();
        public Zone ToZone { get; }
        public bool IsSingleMove => Cards.Count() == 1;
        public override bool CanBeExecuted => Cards.Count() > 0;

        /*******************************************************************/
        public MoveCardsGameAction(IEnumerable<Card> cards, Zone zone, bool isFaceDown = false)
        {
            ToZone = zone;
            _cards = cards;
            _isFaceDown = isFaceDown;
        }

        public MoveCardsGameAction(Card card, Zone zone, bool isFaceDown = false) : this(new[] { card }, zone, isFaceDown) { }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            _cardsWithUndoState = _cards.ToDictionary(card => card, card => (card.CurrentZone, card.FaceDown.IsActive));
            foreach (Card card in Cards)
            {
                card.FaceDown.UpdateValueTo(_isFaceDown);
                card.CurrentZone?.RemoveCard(card);
                ToZone.AddCard(card);
            }

            await _moveCardPresenter.PlayAnimationWith(this);
        }

        public override async Task Undo()
        {
            foreach (Card card in Cards)
            {
                card.FaceDown.UpdateValueTo(_cardsWithUndoState[card].faceDown);
                ToZone.RemoveCard(card);
                _cardsWithUndoState[card].zone.AddCard(card);
            }

            await _moveCardPresenter.PlayAnimationWith(this);
        }
    }
}

