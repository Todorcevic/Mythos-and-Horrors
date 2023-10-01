using NUnit.Framework;
using Zenject;

namespace Tuesday.Tests
{
    public abstract class OneTimeAutoInject
    {
        public DiContainer Container => TestsInstaller.Container;

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            Container.Inject(this);
        }
    }
}