using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class PayResourceGameAction : GameAction
    {
        public Investigator Investigator { get; private set; }
        public int Amount { get; private set; }
        public override bool CanBeExecuted => Investigator.IsInPlay && Amount > 0;

        /*******************************************************************/
        public PayResourceGameAction SetWith(Investigator investigator, int amount)
        {
            Investigator = investigator;
            Amount = amount;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(Investigator.Resources, Amount).Execute();
        }
    }
}
