using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class HideCardsGameAction : GameAction
    {
        public IEnumerable<Card> Cards { get; private set; }
        public override bool CanUndo => false;
        public override bool CanBeExecuted => Cards.Any(card => !card.FaceDown.IsActive);

        /*******************************************************************/
        public HideCardsGameAction SetWith(Card card) => SetWith(new[] { card });

        public HideCardsGameAction SetWith(IEnumerable<Card> cards)
        {
            Cards = cards;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Cards.Select(card => card.FaceDown), true).Start();
        }
    }
}
