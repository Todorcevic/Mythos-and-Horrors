using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class InitialDrawGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Investigator Investigator { get; }
        public override bool CanBeExecuted => Investigator.HandZone.Cards.Count < GameValues.INITIAL_DRAW_SIZE;

        /*******************************************************************/
        public InitialDrawGameAction(Investigator investigator)
        {
            Investigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Card nextDraw = Investigator.DeckZone.Cards.Last();
            if (nextDraw is IFlaw)
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
