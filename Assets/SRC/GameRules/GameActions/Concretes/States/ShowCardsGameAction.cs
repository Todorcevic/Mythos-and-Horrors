using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class ShowCardsGameAction : GameAction
    {
        public IEnumerable<Card> Cards { get; private set; }
        public override bool CanUndo => false;

        /*******************************************************************/
        public ShowCardsGameAction SetWith(Card card) => SetWith(new[] { card });

        public ShowCardsGameAction SetWith(IEnumerable<Card> cards)
        {
            Cards = cards;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Cards.Select(card => card.FaceDown), false).Execute();
        }
    }
}
