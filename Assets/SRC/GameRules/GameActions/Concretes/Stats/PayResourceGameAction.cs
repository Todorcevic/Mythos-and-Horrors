using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PayResourceGameAction : GameAction
    {
        [Inject] private readonly GameActionProvider _gameActionFactory;

        public Investigator Investigator { get; }
        public int Amount { get; }

        /*******************************************************************/
        public PayResourceGameAction(Investigator investigator, int amount)
        {
            Investigator = investigator;
            Amount = amount;
            CanBeExecuted = Amount > 0;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionFactory.Create(new DecrementStatGameAction(Investigator.Resources, Amount));
        }
    }
}
