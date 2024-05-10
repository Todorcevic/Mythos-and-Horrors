using DG.Tweening;
using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public abstract class TestCommon : SceneTestFixture
    {
        private const float TIMEOUT = 3f;
        [Inject] protected readonly GameActionsProvider _gameActionsProvider;
        [Inject] protected readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] protected readonly ChaptersProvider _chaptersProvider;
        [Inject] protected readonly ChallengeTokensProvider _challengeTokensProvider;
        [Inject] protected readonly CardsProvider _cardsProvider;
        [Inject] protected readonly ReactionablesProvider _reactionablesProvider;
        [Inject] protected readonly BuffsProvider _buffsProvider;

        [Inject] protected readonly CardViewsManager _cardViewsManager;
        [Inject] protected readonly AvatarViewsManager _avatarViewsManager;
        [Inject] protected readonly ZoneViewsManager _zoneViewsManager;
        [Inject] protected readonly AreaInvestigatorViewsManager _areaInvestigatorViewsManager;

        [Inject] protected readonly ShowHistoryComponent _showHistoryComponent;
        [Inject] protected readonly RegisterChapterComponent _registerChapterComponent;
        [Inject] protected readonly TokensPileComponent tokensPileComponent;
        [Inject] protected readonly MainButtonComponent _mainButtonComponent;
        [Inject] protected readonly IOActivatorComponent _ioActivatorComponent;
        [Inject] protected readonly ChallengeBagComponent _challengeBagComponent;
        [Inject] protected readonly CardViewGeneratorComponent _cardViewGeneratorComponent;

        [Inject] protected readonly SwapInvestigatorHandler _swapInvestigatorPresenter;
        [Inject] protected readonly MultiEffectHandler _multiEffectHandler;
        [Inject] protected readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] protected readonly CardLoaderUseCase _cardLoaderUseCase;
        [Inject] protected readonly UndoGameActionButton _undoGameActionButton;
        private static string currentSceneName;

        protected virtual bool DEBUG_MODE => false;
        protected abstract string SCENE_NAME { get; }
        protected abstract string JSON_SAVE_DATA_PATH { get; }

        /*******************************************************************/
        [UnitySetUp]
        public override IEnumerator SetUp()
        {
            if (currentSceneName == SCENE_NAME)
            {
                SceneContainer?.Inject(this);
                yield break;
            }
            yield return base.TearDown();
            InstallerToScene();
            yield return LoadScene(SCENE_NAME, InstallerToTests);
            LoadSceneSettings();
            AlwaysHistoryPanelClick().AsTask();
            AlwaysRegisterPanelClick().AsTask();
        }

        [UnityTearDown]
        public override IEnumerator TearDown()
        {
            if (DEBUG_MODE) yield break;
            yield return _gameActionsProvider.Rewind().AsCoroutine();
        }

        private void LoadSceneSettings()
        {
            if (!DEBUG_MODE) Time.timeScale = 64;
            DOTween.SetTweensCapacity(1250, 312);
            currentSceneName = SCENE_NAME;
        }

        private void InstallerToScene()
        {
            StaticContext.Container.BindInstance(JSON_SAVE_DATA_PATH).WhenInjectedInto<DataSaveUseCase>();
            StaticContext.Container.BindInstance(false).WhenInjectedInto<InitializerComponent>();
        }

        private void InstallerToTests()
        {
            SceneContainer.Bind<PreparationSceneCORE1>().AsSingle();
            SceneContainer.Bind<PreparationSceneCORE2>().AsSingle();
            SceneContainer.Bind<PreparationSceneCORE3>().AsSingle();
        }

        /*******************************************************************/
        protected bool IsClickable(Card card) =>
            _cardViewsManager.GetCardView(card).GetPrivateMember<CardSensorController>("_cardSensor").IsClickable;

        protected IEnumerator WaitLoadImages() => new WaitUntil(ImageExtension.IsAllDone);

        protected IEnumerator PressAnyKey() => new WaitUntil(() => Input.anyKeyDown);

        protected IEnumerator AlwaysHistoryPanelClick()
        {
            Button historyButton = _showHistoryComponent.GetPrivateMember<Button>("_button");
            while (!historyButton.interactable) yield return null;

            if (historyButton.interactable) historyButton.onClick.Invoke();
            else throw new TimeoutException("History Button Not become clickable");

            while (historyButton.interactable) yield return null;
            yield return AlwaysHistoryPanelClick();
        }

        protected IEnumerator AlwaysRegisterPanelClick()
        {
            Button registerButton = _registerChapterComponent.GetPrivateMember<Button>("_button");
            while (!registerButton.interactable) yield return null;

            if (registerButton.interactable) registerButton.onClick.Invoke();
            else throw new TimeoutException("Register Button Not become clickable");

            while (registerButton.interactable) yield return null;
            yield return AlwaysRegisterPanelClick();
        }

        protected IEnumerator WaitToMainButtonClick()
        {
            float startTime = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup - startTime < TIMEOUT && !_mainButtonComponent.GetPrivateMember<bool>("IsActivated")) yield return null;

            if (_mainButtonComponent.GetPrivateMember<bool>("IsActivated")) _mainButtonComponent.OnMouseUpAsButton();
            else throw new TimeoutException("Main Button Not become clickable");

            yield return DotweenExtension.WaitForAnimationsComplete().AsCoroutine();
        }

        protected IEnumerator WaitToUndoClick()
        {
            while (!_undoGameActionButton.GetPrivateMember<bool>("_isPlayable")) yield return null;

            if (_undoGameActionButton.GetPrivateMember<bool>("_isPlayable")) _undoGameActionButton.OnPointerClick(null);
            else throw new TimeoutException("Main Button Not become clickable");

            yield return DotweenExtension.WaitForAnimationsComplete().AsCoroutine();
        }

        protected IEnumerator WaitToClick(Card card)
        {
            float startTime = Time.realtimeSinceStartup;
            CardSensorController cardSensor = _cardViewsManager.GetCardView(card).GetPrivateMember<CardSensorController>("_cardSensor");

            while (Time.realtimeSinceStartup - startTime < TIMEOUT && !cardSensor.IsClickable) yield return null;

            if (cardSensor.IsClickable) cardSensor.OnMouseUpAsButton();
            else throw new TimeoutException($"Card: {card.Info.Code} Not become clickable");
            yield return DotweenExtension.WaitForAnimationsComplete().AsCoroutine();
        }

        protected IEnumerator WaitToCloneClick(int clonePosition)
        {
            float startTime = Time.realtimeSinceStartup;
            while (_gameActionsProvider.CurrentInteractable == null) yield return null;
            while (_multiEffectHandler.GetPrivateMember<List<IPlayable>>("cardViewClones") == null) yield return null;

            CardView cardView = _multiEffectHandler.GetPrivateMember<List<IPlayable>>("cardViewClones")[clonePosition] as CardView;
            CardSensorController cardSensor = cardView.GetPrivateMember<CardSensorController>("_cardSensor");

            while (Time.realtimeSinceStartup - startTime < TIMEOUT && !cardSensor.IsClickable) yield return null;

            if (cardSensor.IsClickable) cardSensor.OnMouseUpAsButton();
            else throw new TimeoutException($"Clone position: {clonePosition} Not become clickable");
            yield return DotweenExtension.WaitForAnimationsComplete().AsCoroutine();
        }

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

        protected IEnumerator WaitToTokenClick()
        {
            float startTime = Time.realtimeSinceStartup;

            while (Time.realtimeSinceStartup - startTime < TIMEOUT && !tokensPileComponent.GetPrivateMember<bool>("_isClickable")) yield return null;

            if (tokensPileComponent.GetPrivateMember<bool>("_isClickable")) tokensPileComponent.OnMouseUpAsButton();
            else throw new TimeoutException($"Tokenpile Not become clickable");
            yield return DotweenExtension.WaitForAnimationsComplete().AsCoroutine();
        }

        public void RevealToken(ChallengeTokenType tokenType)
        {
            _reactionablesProvider.CreateReaction<RevealChallengeTokenGameAction>((_) => true, Reveal, true);

            async Task Reveal(GameAction gameAction)
            {
                if (gameAction is not RevealChallengeTokenGameAction revealChallengeTokenGameAction) return;
                ChallengeToken token = _challengeTokensProvider.ChallengeTokensInBag
                    .Find(challengeToken => challengeToken.TokenType == tokenType);
                revealChallengeTokenGameAction.SetChallengeToken(token);
                _reactionablesProvider.RemoveReaction<RevealChallengeTokenGameAction>(Reveal);
                await Task.CompletedTask;
            }
        }
    }
}
