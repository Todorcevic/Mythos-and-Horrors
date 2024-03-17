using DG.Tweening;
using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class TestBase : SceneTestFixture
    {
        private const float TIMEOUT = 3f;

        protected virtual bool DEBUG_MODE => false;

        /*******************************************************************/
        [UnitySetUp]
        public override IEnumerator SetUp()
        {
            yield return base.SetUp();
            DOTween.SetTweensCapacity(500, 312);
            if (!DEBUG_MODE) WithoutAnimations();
            InstallerToScene();
            yield return LoadScene("GamePlay", InstallerToTests);
        }

        [UnityTearDown]
        public override IEnumerator TearDown()
        {
            //yield return WaitLoadImages();
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
        }

        /*******************************************************************/
        protected IEnumerator PressAnyKey() => new WaitUntil(() => Input.anyKeyDown);

        protected IEnumerator WaitLoadImages() => new WaitUntil(ImageExtension.IsAllDone);

        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        protected IEnumerator PlayThisInvestigator(Investigator investigator)
        {
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(investigator.InvestigatorCard, investigator.InvestigatorZone)).AsCoroutine();
        }

        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        protected IEnumerator PlayAllInvestigators()
        {
            foreach (Investigator investigator in _investigatorsProvider.Investigators)
                yield return PlayThisInvestigator(investigator);
        }

        [Inject] private readonly ShowHistoryComponent _showHistoryComponent;
        protected IEnumerator WaitToHistoryPanelClick()
        {
            float startTime = Time.realtimeSinceStartup;
            Button historyButton = _showHistoryComponent.GetPrivateMember<Button>("_button");

            while (Time.realtimeSinceStartup - startTime < TIMEOUT && !historyButton.interactable)
                yield return null;

            if (historyButton.interactable) historyButton.onClick.Invoke();
            else throw new TimeoutException("History Button Not become clickable");
            yield return DotweenExtension.WaitForAnimationsComplete().AsCoroutine();
        }

        [Inject] private readonly CardViewsManager _cardViewsManager;
        protected IEnumerator WaitToClick(Card card)
        {
            float startTime = Time.realtimeSinceStartup;
            CardSensorController cardSensor = _cardViewsManager.GetCardView(card).GetPrivateMember<CardSensorController>("_cardSensor");

            while (Time.realtimeSinceStartup - startTime < TIMEOUT && !cardSensor.IsClickable) yield return null;

            if (cardSensor.IsClickable) cardSensor.OnMouseUpAsButton();
            else throw new TimeoutException($"Card: {card.Info.Code} Not become clickable");
            yield return DotweenExtension.WaitForAnimationsComplete().AsCoroutine();
        }

        [Inject] private readonly MultiEffectHandler _multiEffectHandler;
        protected IEnumerator WaitToCloneClick(Effect effect)
        {
            float startTime = Time.realtimeSinceStartup;

            while (_multiEffectHandler.GetPrivateMember<List<IPlayable>>("cardViewClones") == null) yield return null;

            List<IPlayable> clones = _multiEffectHandler.GetPrivateMember<List<IPlayable>>("cardViewClones");
            CardView cardView = clones.Find(playable => playable.EffectsSelected.Contains(effect)) as CardView;
            CardSensorController cardSensor = cardView.GetPrivateMember<CardSensorController>("_cardSensor");

            while (Time.realtimeSinceStartup - startTime < TIMEOUT && !cardSensor.IsClickable) yield return null;

            if (cardSensor.IsClickable) cardSensor.OnMouseUpAsButton();
            else throw new TimeoutException($"Clone with Effect: {effect.Description} Not become clickable");
            yield return DotweenExtension.WaitForAnimationsComplete().AsCoroutine();
        }

        [Inject] private readonly TokensPileComponent tokensPileComponent;
        protected IEnumerator WaitToTokenClick()
        {
            float startTime = Time.realtimeSinceStartup;

            while (Time.realtimeSinceStartup - startTime < TIMEOUT && !tokensPileComponent.GetPrivateMember<bool>("_isClickable")) yield return null;

            if (tokensPileComponent.GetPrivateMember<bool>("_isClickable")) tokensPileComponent.OnMouseUpAsButton();
            else throw new TimeoutException($"Tokenpile Not become clickable");
            yield return DotweenExtension.WaitForAnimationsComplete().AsCoroutine();
        }
    }
}
