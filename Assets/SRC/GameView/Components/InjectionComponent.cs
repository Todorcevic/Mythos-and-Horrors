using Zenject;

namespace MythsAndHorrors.PlayMode
{
    public class InjectionComponent : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Install<InjectionService>();
        }
    }
}