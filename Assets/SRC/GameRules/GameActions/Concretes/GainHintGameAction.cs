using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public class GainHintGameAction : GameAction
    {
        public Investigator Investigator { get; private set; }
        public Card FromCard { get; private set; }
        public int Amount { get; private set; }

        /*******************************************************************/
        public async Task Run(Investigator investigator, Card fromCard, int amount)
        {
            Investigator = investigator;
            FromCard = fromCard;
            Amount = amount;
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Investigator.Hints.Increase(Amount);
            await Task.CompletedTask;
        }
    }
}
