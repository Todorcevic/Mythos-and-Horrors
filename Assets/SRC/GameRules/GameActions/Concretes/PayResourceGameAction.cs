using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class PayResourceGameAction : GameAction
    {
        private int _amount;
        [Inject] private readonly IResourceMover _resourceMover;

        public Card Card { get; private set; }

        /*******************************************************************/
        public async Task Run(Card card, int amount)
        {
            Card = card;
            _amount = amount;
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Card.RemoveResources(_amount);
            await _resourceMover.RemoveResource(Card, _amount);
        }
    }
}
