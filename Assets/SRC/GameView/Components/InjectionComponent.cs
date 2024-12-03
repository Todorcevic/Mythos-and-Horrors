using Zenject;

namespace MythosAndHorrors.GameView
{
    public class InjectionComponent : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Install<InjectionService>();
        }
    }
}