using GameRules;
using Zenject;

namespace Tuesday
{
    public class InjectionService : Installer
    {
        public override void InstallBindings()
        {
            InstallView();
            InstallRules();
        }

        private void InstallView()
        {
            string gameViewNameSpace = typeof(CardView).Namespace;

            /*** Managers ***/
            Container.Bind(x => x.AllNonAbstractClasses()
            .InNamespace(gameViewNameSpace).WithSuffix("Manager")).AsSingle();

            /*** Services ***/
            Container.Bind(x => x.AllNonAbstractClasses()
            .InNamespace(gameViewNameSpace).WithSuffix("Service")).AsSingle();

            /*** Presenters ***/
            Container.Bind(x => x.AllNonAbstractClasses()
            .InNamespace(gameViewNameSpace).WithSuffix("Presenter")).AsSingle();

            Container.Bind(x => x.AllInterfaces()).To(x => x.AllNonAbstractClasses()
            .InNamespace(gameViewNameSpace).WithSuffix("Presenter")).FromResolve();

            /*** UseCases ***/
            Container.Bind(x => x.AllNonAbstractClasses()
            .InNamespace(gameViewNameSpace).WithSuffix("UseCase")).AsSingle();
        }

        private void InstallRules()
        {
            string gameRulesNameSpace = typeof(Card).Namespace;

            /*** Actions ***/
            Container.Bind(x => x.AllNonAbstractClasses()
            .InNamespace(gameRulesNameSpace).WithSuffix("Action")).AsTransient();

            /*** Services ***/
            Container.Bind(x => x.AllNonAbstractClasses()
            .InNamespace(gameRulesNameSpace).WithSuffix("Service")).AsSingle();

            Container.Bind(x => x.AllInterfaces()).To(x => x.AllNonAbstractClasses()
            .InNamespace(gameRulesNameSpace).WithSuffix("Service")).FromResolve();

            /*** Repositories ***/
            Container.Bind(x => x.AllNonAbstractClasses()
            .InNamespace(gameRulesNameSpace).WithSuffix("Repository")).AsSingle();

            Container.Bind(x => x.AllInterfaces()).To(x => x.AllNonAbstractClasses()
            .InNamespace(gameRulesNameSpace).WithSuffix("Repository")).FromResolve();

            /*** Factories ***/
            Container.Bind(x => x.AllNonAbstractClasses()
            .InNamespace(gameRulesNameSpace).WithSuffix("Factory")).AsSingle();
        }
    }
}
