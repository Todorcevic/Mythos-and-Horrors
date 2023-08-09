using Zenject;

namespace GameView
{
    public class InjectionComponent : MonoInstaller
    {
        private string AssamblyName => GetType().Namespace;

        /*******************************************************************/
        public override void InstallBindings()
        {
            /*** Managers ***/
            Container.Bind(x => x.AllNonAbstractClasses()
           .InNamespace(AssamblyName).WithSuffix("Manager")).AsSingle();
        }
    }
}