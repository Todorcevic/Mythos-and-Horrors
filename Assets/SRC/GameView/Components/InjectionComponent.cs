using Zenject;

namespace GameView
{
    public class InjectionComponent : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Install<InjectionService>();
        }
    }
}