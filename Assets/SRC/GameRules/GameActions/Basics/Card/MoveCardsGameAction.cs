using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class MoveCardsGameAction : GameAction
    {
        private Dictionary<Card, (Zone zone, bool faceDown)> _cardsWithUndoState;
        public Dictionary<Card, (Zone zone, bool faceDown)> AllMoves { get; private set; }
        public IEnumerable<Card> Cards => AllMoves.Keys.ToList();
        public Card SingleCard => Cards.Unique();
        public bool IsSingleMove => Cards.Count() == 1;
        public override bool CanBeExecuted => Cards.Count() > 0;
        public override bool CanUndo => !Cards.Any(card => _cardsWithUndoState[card].faceDown && !AllMoves[card].faceDown);

        /*******************************************************************/
        public MoveCardsGameAction SetWith(Card card, Zone zone, bool isFaceDown = false) =>
            SetWith(new Dictionary<Card, (Zone zone, bool faceDown)> { { card, (zone, isFaceDown) } });

        public MoveCardsGameAction SetWith(IEnumerable<Card> cards, Zone zone, bool isFaceDown = false) =>
            SetWith(cards.ToDictionary(card => card, card => (zone, isFaceDown)));

        public MoveCardsGameAction SetWith(Dictionary<Card, Zone> allMoves) =>
            SetWith(allMoves.ToDictionary(kv => kv.Key, kv => (kv.Value, false)));

        public MoveCardsGameAction SetWith(Dictionary<Card, (Zone zone, bool faceDown)> allMoves)
        {
            AllMoves = allMoves;
            return this;
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

            await Task.CompletedTask;
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
            await base.Undo();
        }

        public Zone GetZoneBeforeMoveFor(Card card) => _cardsWithUndoState[card].zone;
    }
}

