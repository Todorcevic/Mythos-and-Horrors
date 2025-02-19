using MythosAndHorrors.GameRules;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class InjectionService : Installer
    {
        public override void InstallBindings()
        {
            InstallView();
            InstallRules();
            InstallSingle();
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

            /*** Handlers ***/
            Container.Bind(x => x.AllNonAbstractClasses()
                .InNamespace(gameViewNameSpace).WithSuffix("Handler")).AsSingle();

            Container.Bind(x => x.AllInterfaces()).To(x => x.AllNonAbstractClasses()
                .InNamespace(gameViewNameSpace).WithSuffix("Handler")).FromResolve();

            /*** UseCases ***/
            Container.Bind(x => x.AllNonAbstractClasses()
                .InNamespace(gameViewNameSpace).WithSuffix("UseCase")).AsSingle();

            /*** Converters ***/
            Container.Bind(x => x.AllNonAbstractClasses()
                .InNamespace(gameViewNameSpace).WithSuffix("Converter")).AsSingle();

            /*** Presenter ***/
            Container.Bind(x => x.AllNonAbstractClasses()
                .InNamespace(gameViewNameSpace).WithSuffix("Presenter")).AsSingle();
        }

        private void InstallRules()
        {
            string gameRulesNameSpace = typeof(Card).Namespace;

            /*** Providers ***/
            Container.Bind(x => x.AllNonAbstractClasses()
            .InNamespace(gameRulesNameSpace).WithSuffix("Provider")).AsSingle();
        }

        private void InstallSingle()
        {
            ZenjectHelper.Initialize(Container);
            Container.Bind<FilesPath>().AsSingle().IfNotBound();
            Container.Bind<IPresenterInteractable>().To<InteractableHub>().AsSingle();
            Container.Bind<IPresenterAnimation>().To<PresenterHub>().AsSingle();
        }
    }
}
