using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class PayHintGameAction : GameAction
    {
        [Inject] private readonly IHintMover _hintMover;

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
            await _hintMover.PayHints(Investigator, ToCard, Amount);
        }
    }
}
