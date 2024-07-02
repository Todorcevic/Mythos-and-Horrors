using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class GainResourceGameAction : GameAction
    {
        public Investigator Investigator { get; private set; }
        public int Amount { get; private set; }

        public override bool CanBeExecuted => Amount > 0;

        /*******************************************************************/
        public GainResourceGameAction SetWith(Investigator investigator, int amount)
        {
            Investigator = investigator;
            Amount = amount;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(Investigator.Resources, Amount).Execute();
        }
    }
}
