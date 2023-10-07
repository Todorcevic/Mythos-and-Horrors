using MythsAndHorrors.GameView;
using NUnit.Framework;
using Zenject;

namespace MythsAndHorrors.GameRules.Tests
{
    public abstract class OneTimeAutoInject
    {
        public DiContainer Container = new();

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            Container.Install<InjectionService>();
            Container.Inject(this);
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
            Container = null;
        }
    }
}