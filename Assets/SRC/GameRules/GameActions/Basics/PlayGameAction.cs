using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class PlayGameAction : GameAction
    {
        [Inject] private readonly IUAActivator _uAActivator;
        private readonly TaskCompletionSource<bool> _waitForSelection = new();

        /*******************************************************************/
        public async Task Run()
        {
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            _uAActivator.HardActivate();
            await _waitForSelection.Task;
            _uAActivator.HardDeactivate();
        }
    }
}

