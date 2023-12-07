using MythsAndHorrors.GameRules;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ActivatorUIPresenter : IUAActivator
    {
        [Inject] private readonly IOActivatorComponent _ioActivatorComponent;

        /*******************************************************************/

        public void ActivateSensor() => _ioActivatorComponent.ActivateSensor();

        public void DesactivateSensor() => _ioActivatorComponent.DesactivateSensor();
    }
}
