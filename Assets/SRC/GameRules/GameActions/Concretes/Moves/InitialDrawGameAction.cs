using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class InitialDrawGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Investigator Investigator { get; }
        public override bool CanBeExecuted => Investigator.HandZone.Cards.Count() < GameValues.INITIAL_DRAW_SIZE;
        public override bool CanUndo => false;

        /*******************************************************************/
        public InitialDrawGameAction(Investigator investigator)
        {
            Investigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Card nextDraw = Investigator.CardAidToDraw ?? throw new System.Exception("No card to draw"); //TODO must shuffle deck with discard
            if (nextDraw.Tags.Contains(Tag.Flaw))
            {
                await _gameActionsProvider.Create(new DiscardGameAction(nextDraw));
                await _gameActionsProvider.Create(new InitialDrawGameAction(Investigator));
                return;
            }

            await _gameActionsProvider.Create(new DrawAidGameAction(Investigator));
            await _gameActionsProvider.Create(new InitialDrawGameAction(Investigator));
        }
    }
}
