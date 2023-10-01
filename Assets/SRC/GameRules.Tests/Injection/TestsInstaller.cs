using NUnit.Framework;
using Zenject;
using Tuesday.GameView;

namespace Tuesday.Tests
{
    [SetUpFixture]
    public class TestsInstaller
    {
        public static DiContainer Container = new();

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            Container.Install<InjectionService>();
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
            Container = null;
        }
    }
}