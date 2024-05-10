using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class MoveCardsGameAction : GameAction
    {
        private Dictionary<Card, (Zone zone, bool faceDown)> _cardsWithUndoState;
        [Inject] private readonly IPresenter<MoveCardsGameAction> _moveCardPresenter;

        public Dictionary<Card, (Zone zone, bool faceDown)> AllMoves { get; }

        public IEnumerable<Card> Cards => AllMoves.Keys.ToList();
        public Card SingleCard => Cards.Unique();
        public bool IsSingleMove => Cards.Count() == 1;
        public override bool CanBeExecuted => Cards.Count() > 0;

        public IEnumerable<Card> EnterPlayCardsAfter => Cards.Where(card => !ZoneType.PlayZone.HasFlag(_cardsWithUndoState[card].zone.ZoneType)
            && ZoneType.PlayZone.HasFlag(AllMoves[card].zone.ZoneType));
        public IEnumerable<Card> ExitPlayCardsAfter => Cards.Where(card => ZoneType.PlayZone.HasFlag(_cardsWithUndoState[card].zone.ZoneType)
            && !ZoneType.PlayZone.HasFlag(AllMoves[card].zone.ZoneType));
        public IEnumerable<Card> EnterPlayCardsBefore => Cards.Where(card => !ZoneType.PlayZone.HasFlag(card.CurrentZone.ZoneType)
            && ZoneType.PlayZone.HasFlag(AllMoves[card].zone.ZoneType));
        public IEnumerable<Card> ExitPlayCardsBefore => Cards.Where(card => ZoneType.PlayZone.HasFlag(card.CurrentZone.ZoneType)
            && !ZoneType.PlayZone.HasFlag(AllMoves[card].zone.ZoneType));

        /*******************************************************************/
        public MoveCardsGameAction(Card card, Zone zone, bool isFaceDown = false) :
            this(new Dictionary<Card, (Zone zone, bool faceDown)> { { card, (zone, isFaceDown) } })
        { }

        public MoveCardsGameAction(IEnumerable<Card> cards, Zone zone, bool isFaceDown = false) :
            this(cards.ToDictionary(card => card, card => (zone, isFaceDown)))
        { }

        public MoveCardsGameAction(Dictionary<Card, Zone> allMoves) :
            this(allMoves.ToDictionary(kv => kv.Key, kv => (kv.Value, false)))
        { }

        public MoveCardsGameAction(Dictionary<Card, (Zone zone, bool faceDown)> allMoves)
        {
            AllMoves = allMoves;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            _cardsWithUndoState = AllMoves.Keys.ToDictionary(card => card, card => (card.CurrentZone, card.FaceDown.IsActive));

            foreach (KeyValuePair<Card, (Zone zone, bool faceDown)> move in AllMoves)
            {
                move.Key.FaceDown.UpdateValueTo(move.Value.faceDown);
                move.Key.CurrentZone?.RemoveCard(move.Key);
                move.Value.zone.AddCard(move.Key);
            }

            await _moveCardPresenter.PlayAnimationWith(this);
        }

        /*******************************************************************/
        public override async Task Undo()
        {
            foreach (KeyValuePair<Card, (Zone zone, bool faceDown)> move in _cardsWithUndoState)
            {
                move.Key.FaceDown.UpdateValueTo(move.Value.faceDown);
                move.Key.CurrentZone?.RemoveCard(move.Key);
                move.Value.zone.AddCard(move.Key);
            }

            await _moveCardPresenter.PlayAnimationWith(this);
        }
    }
}

