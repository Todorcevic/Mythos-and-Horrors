using DG.Tweening;
using MythsAndHorrors.GameRules;
using MythsAndHorrors.GameView;
using System;
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
            float timeout = 1f;
            float startTime = Time.realtimeSinceStartup;
            Button historyButton = _showHistoryComponent.GetPrivateMember<Button>("_button");

            while (Time.realtimeSinceStartup - startTime < timeout && !historyButton.interactable)
                yield return null;

            if (historyButton.interactable) historyButton.onClick.Invoke();
            else throw new TimeoutException("History Button Not become clickable");
        }

        [Inject] private readonly CardViewsManager _cardViewsManager;
        protected IEnumerator WaitToClick(Card card)
        {
            float timeout = 1f;
            float startTime = Time.realtimeSinceStartup;
            CardSensorController cardSensor = _cardViewsManager.GetCardView(card).GetComponentInChildren<CardSensorController>();

            while (Time.realtimeSinceStartup - startTime < timeout && !cardSensor.IsClickable) yield return null;

            if (cardSensor.IsClickable) cardSensor.OnMouseUpAsButton();
            else throw new TimeoutException($"Card: {card.Info.Code} Not become clickable");
        }
    }
}
