using GameRules;
using Zenject;

namespace GameView
{
    public class InjectionComponent : MonoInstaller
    {
        private string GameViewAssamblyName => GetType().Namespace;
        private string GameRulesAssamblyName => typeof(CardRepository).Namespace;

        /*******************************************************************/
        public override void InstallBindings()
        {
            /*** Managers ***/
            Container.Bind(x => x.AllNonAbstractClasses()
           .InNamespace(GameViewAssamblyName).WithSuffix("Manager")).AsSingle();

            /*** Presenters ***/
            Container.Bind(x => x.AllInterfaces()).To(x => x.AllNonAbstractClasses()
            .InNamespace(GameViewAssamblyName).WithSuffix("Presenter")).AsSingle();

            /*** Actions ***/
            Container.Bind(x => x.AllNonAbstractClasses()
           .InNamespace(GameRulesAssamblyName).WithSuffix("Action")).AsTransient();

            /*** UseCases ***/
            Container.Bind(x => x.AllNonAbstractClasses()
           .InNamespace(GameRulesAssamblyName).WithSuffix("UseCase")).AsSingle();

            /*** Repositories ***/
            Container.Bind(x => x.AllNonAbstractClasses()
           .InNamespace(GameRulesAssamblyName).WithSuffix("Repository")).AsSingle();

            /*** Factories ***/
            Container.Bind(x => x.AllNonAbstractClasses()
           .InNamespace(GameRulesAssamblyName).WithSuffix("Factory")).AsSingle();
        }
    }
}