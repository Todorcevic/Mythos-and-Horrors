using Zenject;

namespace Tuesday.GameView
{
    public class InjectionComponent : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Install<InjectionService>();
        }
    }
}