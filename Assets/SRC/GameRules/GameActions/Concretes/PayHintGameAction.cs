using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public class PayHintGameAction : GameAction
    {
        public Investigator Investigator { get; private set; }
        public Card ToCard { get; private set; }
        public int Amount { get; private set; }

        /*******************************************************************/
        public async Task Run(Investigator investigator, Card toCard, int amount)
        {
            Investigator = investigator;
            ToCard = toCard;
            Amount = amount;
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Investigator.Hints.Decrease(Amount);
            await Task.CompletedTask;
        }
    }
}
