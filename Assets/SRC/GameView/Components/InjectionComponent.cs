using GameRules;
using System.ComponentModel;
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
            /********************************* VIEW **********************************/
            /*** Managers ***/
            Container.Bind(x => x.AllNonAbstractClasses()
            .InNamespace(GameViewAssamblyName).WithSuffix("Manager")).AsSingle();

            /*** Services ***/
            Container.Bind(x => x.AllNonAbstractClasses()
            .InNamespace(GameViewAssamblyName).WithSuffix("Service")).AsSingle();

            Container.Bind(x => x.AllInterfaces()).To(x => x.AllNonAbstractClasses()
            .InNamespace(GameViewAssamblyName).WithSuffix("Service")).FromResolve();

            /*** Presenters ***/
            Container.Bind(x => x.AllNonAbstractClasses()
            .InNamespace(GameViewAssamblyName).WithSuffix("Presenter")).AsSingle();

            Container.Bind(x => x.AllInterfaces()).To(x => x.AllNonAbstractClasses()
            .InNamespace(GameViewAssamblyName).WithSuffix("Presenter")).FromResolve();

            /****************************** APPLICATION ******************************/
            /*** Actions ***/
            Container.Bind(x => x.AllNonAbstractClasses()
            .InNamespace(GameRulesAssamblyName).WithSuffix("Action")).AsTransient();

            /*** UseCases ***/
            Container.Bind(x => x.AllNonAbstractClasses()
            .InNamespace(GameRulesAssamblyName).WithSuffix("UseCase")).AsSingle();

            /*** Services ***/
            Container.Bind(x => x.AllNonAbstractClasses()
            .InNamespace(GameRulesAssamblyName).WithSuffix("Service")).AsSingle();

            Container.Bind(x => x.AllInterfaces()).To(x => x.AllNonAbstractClasses()
            .InNamespace(GameRulesAssamblyName).WithSuffix("Service")).FromResolve();

            /*** Repositories ***/
            Container.Bind(x => x.AllNonAbstractClasses()
            .InNamespace(GameRulesAssamblyName).WithSuffix("Repository")).AsSingle();

            Container.Bind(x => x.AllInterfaces()).To(x => x.AllNonAbstractClasses()
            .InNamespace(GameRulesAssamblyName).WithSuffix("Repository")).FromResolve();

            /*** Factories ***/
            Container.Bind(x => x.AllNonAbstractClasses()
            .InNamespace(GameRulesAssamblyName).WithSuffix("Factory")).AsSingle();
        }
    }
}