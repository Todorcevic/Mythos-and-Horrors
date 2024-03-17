using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class GainResourceGameAction : GameAction
    {
        private IncrementStatGameAction _incrementStatGameAction;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Investigator Investigator { get; }
        public int Amount { get; }

        /*******************************************************************/
        public GainResourceGameAction(Investigator investigator, int amount)
        {
            Investigator = investigator;
            Amount = amount;
            CanBeExecuted = Amount > 0;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            _incrementStatGameAction = await _gameActionsProvider.Create(new IncrementStatGameAction(Investigator.Resources, Amount));
        }

        protected override async Task UndoThisLogic()
        {
            await _incrementStatGameAction.Undo();
        }
    }
}
