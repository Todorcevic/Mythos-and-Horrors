using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class InitialDrawGameAction : GameAction
    {
        public Investigator Investigator { get; private set; }
        public override bool CanBeExecuted => Investigator.HandZone.Cards.Count() < GameValues.INITIAL_DRAW_SIZE;
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
            Card nextDraw = Investigator.CardAidToDraw ?? throw new System.Exception("No card to draw"); //TODO must shuffle deck with discard
            if (nextDraw.Tags.Contains(Tag.Weakness))
            {
                await _gameActionsProvider.Create<DiscardGameAction>().SetWith(nextDraw).Execute();
                await _gameActionsProvider.Create<InitialDrawGameAction>().SetWith(Investigator).Execute();
                return;
            }

            await _gameActionsProvider.Create<DrawAidGameAction>().SetWith(Investigator).Execute();
            await _gameActionsProvider.Create<InitialDrawGameAction>().SetWith(Investigator).Execute();
        }
    }
}
