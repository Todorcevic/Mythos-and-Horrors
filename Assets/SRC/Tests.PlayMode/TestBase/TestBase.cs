using DG.Tweening;
using MythsAndHorrors.GameRules;
using MythsAndHorrors.GameView;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Zenject;

namespace MythsAndHorrors.PlayMode.Tests
{
    public class TestBase : SceneTestFixture
    {
        protected virtual bool DEBUG_MODE => false;

        /*******************************************************************/
        [UnitySetUp]
        public override IEnumerator SetUp()
        {
            yield return base.SetUp();
            DOTween.SetTweensCapacity(200, 125);
            if (!DEBUG_MODE) WithoutAnimations();
            InstallerToScene();
            yield return LoadScene("GamePlay", InstallerToTests);
        }

        [UnityTearDown]
        public override IEnumerator TearDown()
        {
            yield return WaitLoadImages();
            SetTimeDefault();
            yield return base.TearDown();
        }

        protected void WithoutAnimations()
        {
            Time.timeScale = 20;
        }

        private void SetTimeDefault()
        {
            Time.timeScale = 1;
        }

        private void InstallerToScene()
        {
            StaticContext.Container.BindInstance(false).WhenInjectedInto<InitializerComponent>();
        }

        private void InstallerToTests()
        {
            SceneContainer.Bind<CardInfoBuilder>().AsTransient();
            SceneContainer.Bind<CardBuilder>().AsSingle();
            SceneContainer.Bind<CardViewBuilder>().AsSingle();
        }

        protected IEnumerator PressAnyKey() => new WaitUntil(() => Input.anyKeyDown);

        protected IEnumerator WaitLoadImages() => new WaitUntil(ImageExtension.IsAllDone);

        [Inject] private readonly ShowHistoryComponent _showHistoryComponent;
        protected IEnumerator WaitToClickHistoryPanel()
        {
            yield return new WaitUntil(() => _showHistoryComponent.GetPrivateMember<Button>("_button").interactable);
            _showHistoryComponent.GetPrivateMember<Button>("_button").onClick.Invoke();
        }

        [Inject] private readonly CardViewsManager _cardViewsManager;
        protected IEnumerator WaitToClick(Card card)
        {
            CardSensorController cardSensor = _cardViewsManager.GetCardView(card).GetComponentInChildren<CardSensorController>();
            yield return new WaitUntil(() => cardSensor.IsClickable);
            cardSensor.OnMouseUpAsButton();
        }
    }
}
