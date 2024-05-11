using MythosAndHorrors.GameView;
using NUnit.Framework;
using Zenject;

namespace MythosAndHorrors.EditMode.Tests
{
    public abstract class SetupAutoInject
    {
        public DiContainer Container;

        [SetUp]
        public virtual void RunBeforeAnyTest()
        {
            Container = new();
            Container.Install<InjectionService>();
            Container.Install<InjectionServiceToTest>();
            Container.Inject(this);
        }

        [TearDown]
        public virtual void RunAfterAnyTest()
        {
            Container = null;
        }
    }
}