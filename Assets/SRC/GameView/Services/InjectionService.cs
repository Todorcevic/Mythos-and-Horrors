using MythosAndHorrors.GameRules;
using System;
using System.Reflection;
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

            InstallGenericPresenterBindings(typeof(IPresenter<>));
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
            Container.Bind<FilesPath>().AsSingle().IfNotBound();
            Container.Bind(typeof(ClickHandler<>)).AsSingle();
            Container.Bind<IInteractablePresenter>().To<InteractablePresenter>().AsSingle();
        }

        private void InstallGenericPresenterBindings(Type interfaceT)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] allTypes = assembly.GetTypes();

            foreach (Type type in allTypes)
            {
                Type[] interfaces = type.GetInterfaces();
                foreach (Type @interface in interfaces)
                {
                    if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == interfaceT)
                    {
                        Type argumentType = @interface.GetGenericArguments()[0];
                        Type genericType = interfaceT.MakeGenericType(argumentType);

                        Container.Bind(genericType).To(type).AsCached();
                    }
                }
            }
        }
    }
}
