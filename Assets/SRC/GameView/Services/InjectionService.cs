using MythsAndHorrors.EditMode;
using Zenject;

namespace MythsAndHorrors.PlayMode
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

            Container.Bind(x => x.AllInterfaces()).To(x => x.AllNonAbstractClasses()
           .InNamespace(gameViewNameSpace).WithSuffix("Manager")).FromResolve();

            /*** Services ***/
            Container.Bind(x => x.AllNonAbstractClasses()
            .InNamespace(gameViewNameSpace).WithSuffix("Service")).AsSingle();

            Container.Bind(x => x.AllInterfaces()).To(x => x.AllNonAbstractClasses()
            .InNamespace(gameViewNameSpace).WithSuffix("Service")).FromResolve();

            /*** Presenters ***/
            Container.Bind(x => x.AllNonAbstractClasses()
            .InNamespace(gameViewNameSpace).WithSuffix("Presenter")).AsSingle();

            Container.Bind(x => x.AllInterfaces()).To(x => x.AllNonAbstractClasses()
            .InNamespace(gameViewNameSpace).WithSuffix("Presenter")).FromResolve();

            /*** UseCases ***/
            Container.Bind(x => x.AllNonAbstractClasses()
            .InNamespace(gameViewNameSpace).WithSuffix("UseCase")).AsSingle();

            /*** Converters ***/
            Container.Bind(x => x.AllNonAbstractClasses()
            .InNamespace(gameViewNameSpace).WithSuffix("Converter")).AsSingle();
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

            /*** Providers ***/
            Container.Bind(x => x.AllNonAbstractClasses()
            .InNamespace(gameRulesNameSpace).WithSuffix("Provider")).AsSingle();

            /*** Factories ***/
            Container.Bind(x => x.AllNonAbstractClasses()
            .InNamespace(gameRulesNameSpace).WithSuffix("Factory")).AsSingle();
        }
    }
}
