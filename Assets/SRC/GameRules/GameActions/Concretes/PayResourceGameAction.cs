using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class PayResourceGameAction : GameAction
    {
        [Inject] private readonly IResourceAnimator _resourceAnimator;

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
            Investigator.Resources.Decrease(Amount);
            await _resourceAnimator.PayResource(Investigator, Amount, ToCard);
        }
    }
}
