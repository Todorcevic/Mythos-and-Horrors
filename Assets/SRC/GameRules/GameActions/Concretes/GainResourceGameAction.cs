using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class GainResourceGameAction : GameAction
    {
        [Inject] private readonly IResourceAnimator _resourceAnimator;

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
            Investigator.Resources.Increase(Amount);
            await _resourceAnimator.GainResource(Investigator, Amount, FromCard);
        }
    }
}
