using MythsAndHorrors.GameView;
using NUnit.Framework;
using Zenject;

namespace MythsAndHorrors.EditMode.Tests
{
    public abstract class SetupAutoInject
    {
        public DiContainer Container;

        [SetUp]
        public virtual void RunBeforeAnyTest()
        {
            Container = new();
            Container.Install<InjectionService>();
            Container.Inject(this);
        }

        [TearDown]
        public virtual void RunAfterAnyTest()
        {
            Container = null;
        }
    }
}