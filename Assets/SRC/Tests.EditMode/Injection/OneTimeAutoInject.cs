using MythsAndHorrors.GameView;
using NUnit.Framework;
using Zenject;

namespace MythsAndHorrors.EditMode.Tests
{
    public abstract class OneTimeAutoInject
    {
        public DiContainer Container;

        [OneTimeSetUp]
        public virtual void RunBeforeAllTest()
        {
            Container = new();
            Container.Install<InjectionService>();
            Container.Inject(this);
        }

        [OneTimeTearDown]
        public virtual void RunAfterAllTest()
        {
            Container = null;
        }
    }
}