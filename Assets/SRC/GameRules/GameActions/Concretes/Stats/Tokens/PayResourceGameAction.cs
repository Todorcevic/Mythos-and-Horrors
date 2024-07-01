using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PayResourceGameAction : GameAction
    {

        public Investigator Investigator { get; }
        public int Amount { get; }
        public override bool CanBeExecuted => Amount > 0;

        /*******************************************************************/
        public PayResourceGameAction(Investigator investigator, int amount)
        {
            Investigator = investigator;
            Amount = amount;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new DecrementStatGameAction(Investigator.Resources, Amount));
        }
    }
}
