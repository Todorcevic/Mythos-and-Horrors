using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class PayResourceGameAction : GameAction
    {
        private int _amount;
        [Inject] private readonly IResourceMover _resourceMover;

        public Adventurer Adventurer { get; private set; }

        /*******************************************************************/
        public async Task Run(Adventurer adventurer, int amount)
        {
            Adventurer = adventurer;
            _amount = amount;
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Adventurer.RemoveResources(_amount);
            await _resourceMover.RemoveResource(Adventurer, _amount);
        }
    }
}
