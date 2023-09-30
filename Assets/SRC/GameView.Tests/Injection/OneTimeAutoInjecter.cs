using NUnit.Framework;
using Zenject;

public abstract class OneTimeAutoInject
{
    public DiContainer Container => TestsInstaller.Container;

    [OneTimeSetUp]
    public virtual void SetUp()
    {
        Container.Inject(this);
    }
}