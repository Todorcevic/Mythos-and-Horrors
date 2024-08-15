using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class InitialDrawGameAction : GameAction
    {
        public Investigator Investigator { get; private set; }
        public override bool CanBeExecuted => Investigator.HandZone.Cards.Count < GameValues.INITIAL_DRAW_SIZE;
        public override bool CanUndo => false;

        /*******************************************************************/
        public InitialDrawGameAction SetWith(Investigator investigator)
        {
            Investigator = investigator;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            IEnumerable<Card> cardsToDraw = Investigator.DeckZone.Cards.Where(card => !card.Tags.Contains(Tag.Weakness))
                .TakeLast(GameValues.INITIAL_DRAW_SIZE - Investigator.HandZone.Cards.Count);

            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardsToDraw, Investigator.HandZone).Execute();
        }
    }
}
