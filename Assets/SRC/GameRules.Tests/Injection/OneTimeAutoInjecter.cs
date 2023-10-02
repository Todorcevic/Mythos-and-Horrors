using NUnit.Framework;
using Zenject;

namespace MythsAndHorrors.GameRules.Tests
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