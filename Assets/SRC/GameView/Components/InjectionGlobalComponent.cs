using Zenject;

namespace MythsAndHorrors.GameView
{
    public class InjectionGlobalComponent : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<InitializeGameUseCase>().AsSingle();
        }
    }
}