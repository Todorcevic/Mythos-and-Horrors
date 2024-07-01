using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class GainResourceGameAction : GameAction
    {

        public Investigator Investigator { get; }
        public int Amount { get; }

        public override bool CanBeExecuted => Amount > 0;

        /*******************************************************************/
        public GainResourceGameAction(Investigator investigator, int amount)
        {
            Investigator = investigator;
            Amount = amount;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(Investigator.Resources, Amount).Start();
        }
    }
}
