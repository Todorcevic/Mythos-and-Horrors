using GameView;
using NUnit.Framework;
using Zenject;

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