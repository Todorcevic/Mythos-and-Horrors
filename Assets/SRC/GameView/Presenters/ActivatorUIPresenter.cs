using MythsAndHorrors.GameRules;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ActivatorUIPresenter : IUAActivator
    {
        private bool isHardDeactivator = false;
        [Inject] private readonly IOActivatorComponent _ioActivatorComponent;

        /*******************************************************************/
        public void HardActivate()
        {
            isHardDeactivator = false;
            _ioActivatorComponent.ActivateSensor();
        }

        public void HardDeactivate()
        {
            isHardDeactivator = true;
            _ioActivatorComponent.DeactivateSensor();
        }

        public void ActivateSensor()
        {
            if (isHardDeactivator) return;
            _ioActivatorComponent.ActivateSensor();
        }

        public void DeactivateSensor()
        {
            if (isHardDeactivator) return;
            _ioActivatorComponent.DeactivateSensor();
        }
    }
}
