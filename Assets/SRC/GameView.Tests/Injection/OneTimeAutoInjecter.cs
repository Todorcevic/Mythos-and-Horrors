using NUnit.Framework;
using Zenject;

public abstract class OneTimeAutoInjected
{
    public DiContainer Container => StaticContext.Container;

    [OneTimeSetUp]
    public virtual void Setup()
    {
        Container.Inject(this);
    }

    [OneTimeTearDown]
    public virtual void Teardown()
    {

    }
}