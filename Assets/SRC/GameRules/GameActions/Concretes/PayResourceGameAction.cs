using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class PayResourceGameAction : GameAction
    {
        private int _amount;
        [Inject] private readonly IResourceMover _resourceMover;

        public Investigator Investigator { get; private set; }

        /*******************************************************************/
        public async Task Run(Investigator investigator, int amount)
        {
            Investigator = investigator;
            _amount = amount;
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Investigator.RemoveResources(_amount);
            await _resourceMover.RemoveResource(Investigator, _amount);
        }
    }
}
