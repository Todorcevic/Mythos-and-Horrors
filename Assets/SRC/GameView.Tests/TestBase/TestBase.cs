using MythsAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythsAndHorrors.Gameview.Tests
{
    [TestFixture]
    public class TestBase : SceneTestFixture
    {
        [UnitySetUp]
        public override IEnumerator SetUp()
        {
            yield return base.SetUp();
            StaticContext.Container.BindInstance(false).WhenInjectedInto<InitializerComponent>();
            yield return LoadScene("GamePlay");
        }

        protected IEnumerator PressAnyKey() => new WaitUntil(() => Input.anyKeyDown);
    }
}
