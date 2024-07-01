using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ShowCardsGameAction : GameAction
    {

        public IEnumerable<Card> Cards { get; }
        public override bool CanUndo => false;
        public override bool CanBeExecuted => Cards.Any(card => card.FaceDown.IsActive);

        /*******************************************************************/
        public ShowCardsGameAction(Card card) : this(new List<Card> { card }) { }

        public ShowCardsGameAction(IEnumerable<Card> cards)
        {
            Cards = cards;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new UpdateStatesGameAction(Cards.Select(card => card.FaceDown), false));
        }
    }
}
