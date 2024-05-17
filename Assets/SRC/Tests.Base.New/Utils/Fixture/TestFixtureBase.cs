using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using System.Collections.Generic;
using System;
using UnityEngine.TestTools;
using Zenject;
using System.Collections;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

namespace MythosAndHorrors.PlayMode.Tests
{
    public abstract class TestFixtureBase : SceneTestFixture
    {
        [Inject] protected readonly PrepareGameRulesUseCase _prepareGameRulesUseCase;
        [Inject] protected readonly GameActionsProvider _gameActionsProvider;
        [Inject] protected readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] protected readonly ChaptersProvider _chaptersProvider;
        [Inject] protected readonly ChallengeTokensProvider _challengeTokensProvider;
        [Inject] protected readonly CardsProvider _cardsProvider;
        [Inject] protected readonly ReactionablesProvider _reactionablesProvider;
        [Inject] protected readonly BuffsProvider _buffsProvider;
        [Inject] private readonly IInteractablePresenter _interactablePresenter;

        private static string currentSceneName;
        protected abstract string JSON_SAVE_DATA_PATH { get; }
        protected virtual string SCENE_NAME => "GamePlayCORE1";
        protected virtual TestsType TestsType => TestsType.Integration;

        /*******************************************************************/
        [UnitySetUp]
        public virtual IEnumerator SetUp()
        {
            if (currentSceneName != JSON_SAVE_DATA_PATH)
            {
                currentSceneName = JSON_SAVE_DATA_PATH;
                if (TestsType == TestsType.Unit)
                {
                    SceneContainer = new();
                    SceneContainer.Install<InjectionService>();
                    InstallerToSceneInUnitMode();
                    InstallFakes();
                    SceneContainer?.Inject(this);
                    _prepareGameRulesUseCase.Execute();
                }
                else
                {
                    ClearContainer();
                    InstallerToSceneInDebugMode();
                    yield return LoadScene(SCENE_NAME);
                    AlwaysHistoryPanelClick(SceneContainer.Resolve<ShowHistoryComponent>()).AsTask();
                    AlwaysRegisterPanelClick(SceneContainer.Resolve<RegisterChapterComponent>()).AsTask();
                    if (TestsType == TestsType.Integration)
                    {
                        Time.timeScale = 64;
                        DOTween.SetTweensCapacity(1250, 312);
                    }
                }
            }
            else SceneContainer?.Inject(this);

            yield return null;
        }

        private void InstallerToSceneInDebugMode()
        {
            StaticContext.Container.BindInstance(JSON_SAVE_DATA_PATH).WhenInjectedInto<DataSaveUseCase>();
            StaticContext.Container.BindInstance(false).WhenInjectedInto<InitializerComponent>();
        }

        private void InstallerToSceneInUnitMode()
        {
            SceneContainer.BindInstance(JSON_SAVE_DATA_PATH).WhenInjectedInto<DataSaveUseCase>();
            SceneContainer.BindInstance(false).WhenInjectedInto<InitializerComponent>();
        }

        private void InstallFakes()
        {
            SceneContainer.Rebind<IInteractablePresenter>().To<FakeInteractablePresenter>().AsCached();
            BindAllFakePresenters();

            static void BindAllFakePresenters()
            {
                IEnumerable<Type> gameActionTypes = typeof(GameAction).Assembly.GetTypes().Where(type => type.IsClass);

                foreach (Type type in gameActionTypes)
                {
                    foreach (FieldInfo campo in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                        .Where(campo => campo.FieldType.IsGenericType
                            && campo.FieldType.GetGenericTypeDefinition() == typeof(IPresenter<>)
                            && campo.FieldType.GetGenericArguments()[0] == type))
                    {
                        Type genericToBind = typeof(FakePresenter<>).MakeGenericType(type);
                        SceneContainer.Rebind(campo.FieldType).To(genericToBind).AsCached();
                    }
                }
            }
        }


        [UnityTearDown]
        public IEnumerator TierDown()
        {
            yield return _gameActionsProvider.Rewind().AsCoroutine().Fast();
        }

        /*******************************************************************/
        protected async Task MustBeRevealedThisToken(ChallengeTokenType tokenType)
        {
            TaskCompletionSource<ChallengeToken> waitForReaction = new();
            _reactionablesProvider.CreateReaction<RevealChallengeTokenGameAction>((_) => true, Reveal, isAtStart: true);
            await waitForReaction.Task;

            /*******************************************************************/
            async Task Reveal(GameAction gameAction)
            {
                if (gameAction is not RevealChallengeTokenGameAction revealChallengeTokenGameAction) return;
                ChallengeToken token = _challengeTokensProvider.ChallengeTokensInBag
                    .Find(challengeToken => challengeToken.TokenType == tokenType);
                revealChallengeTokenGameAction.SetChallengeToken(token);
                waitForReaction.SetResult(revealChallengeTokenGameAction.ChallengeTokenRevealed);
                _reactionablesProvider.RemoveReaction<RevealChallengeTokenGameAction>(Reveal);
                await Task.CompletedTask;
            }
        }

        protected async Task<ChallengeToken> CaptureTokenWhenReveled()
        {
            TaskCompletionSource<ChallengeToken> waitForReaction = new();
            _reactionablesProvider.CreateReaction<RevealChallengeTokenGameAction>((_) => true, Reveal, isAtStart: false);
            return await waitForReaction.Task;

            /*******************************************************************/
            async Task Reveal(GameAction gameAction)
            {
                if (gameAction is not RevealChallengeTokenGameAction revealChallengeTokenGameAction) return;
                waitForReaction.SetResult(revealChallengeTokenGameAction.ChallengeTokenRevealed);
                _reactionablesProvider.RemoveReaction<RevealChallengeTokenGameAction>(Reveal);
                await Task.CompletedTask;
            }
        }

        protected Task<int> CaptureTokenValue(Investigator investigator) => Task.Run(async () =>
            {
                ChallengeToken token = await CaptureTokenWhenReveled();
                return token.Value(investigator);
            });

        protected async Task<ChallengePhaseGameAction> CaptureResolvingChallenge()
        {
            TaskCompletionSource<ChallengePhaseGameAction> waitForReaction = new();
            _reactionablesProvider.CreateReaction<ResultChallengeGameAction>((_) => true, ResolveChallenge, isAtStart: false);
            return await waitForReaction.Task;

            /*******************************************************************/
            async Task ResolveChallenge(GameAction gameAction)
            {
                if (gameAction is not ResultChallengeGameAction resolveChallengeGameAction) return;
                waitForReaction.SetResult(resolveChallengeGameAction.ChallengePhaseGameAction);
                _reactionablesProvider.RemoveReaction<ResultChallengeGameAction>(ResolveChallenge);
                await Task.CompletedTask;
            }
        }

        protected Task<(int totalTokensAmount, int totalTokensValue)> CaptureTotalTokensRevelaed() => Task.Run(async () =>
        {
            ChallengePhaseGameAction challenge = await CaptureResolvingChallenge();
            return (challenge.TokensRevealed.Count(), challenge.TotalTokenValue);
        });

        protected Task<int> CaptureTotalChallengeValue() => Task.Run(async () =>
        {
            ChallengePhaseGameAction challenge = await CaptureResolvingChallenge();
            return challenge.TotalChallengeValue;
        });

        /*******************************************************************/
        private const float TIMEOUT = 3f;
        protected IEnumerator ClickedIn(Card card)
        {
            if (_interactablePresenter is FakeInteractablePresenter fakeInteractable)
                yield return fakeInteractable.ClickedIn(card);
            else if (TestsType == TestsType.Integration)
            {
                CardViewsManager _cardViewsManager = SceneContainer.Resolve<CardViewsManager>();
                float startTime = Time.realtimeSinceStartup;
                CardSensorController cardSensor = _cardViewsManager.GetCardView(card).GetPrivateMember<CardSensorController>("_cardSensor");

                while (Time.realtimeSinceStartup - startTime < TIMEOUT && !cardSensor.IsClickable) yield return null;

                if (cardSensor.IsClickable) cardSensor.OnMouseUpAsButton();
                else throw new TimeoutException($"Card: {card.Info.Code} Not become clickable");
                yield return DotweenExtension.WaitForAnimationsComplete().AsCoroutine();
            }
        }

        protected IEnumerator ClickedClone(Card card, int position)
        {
            if (_interactablePresenter is FakeInteractablePresenter fakeInteractable)
                yield return fakeInteractable.ClickedIn(card, position);
            else if (TestsType == TestsType.Integration)
            {
                yield return ClickedIn(card);
                MultiEffectHandler _multiEffectHandler = SceneContainer.Resolve<MultiEffectHandler>();
                float startTime = Time.realtimeSinceStartup;
                while (_gameActionsProvider.CurrentInteractable == null) yield return null;
                while (_multiEffectHandler.GetPrivateMember<List<IPlayable>>("cardViewClones") == null) yield return null;

                CardView cardView = _multiEffectHandler.GetPrivateMember<List<IPlayable>>("cardViewClones")[position] as CardView;
                CardSensorController cardSensor = cardView.GetPrivateMember<CardSensorController>("_cardSensor");

                while (Time.realtimeSinceStartup - startTime < TIMEOUT && !cardSensor.IsClickable) yield return null;

                if (cardSensor.IsClickable) cardSensor.OnMouseUpAsButton();
                else throw new TimeoutException($"Clone position: {position} Not become clickable");
                yield return DotweenExtension.WaitForAnimationsComplete().AsCoroutine();
            }
        }

        protected IEnumerator ClickedMainButton()
        {
            if (_interactablePresenter is FakeInteractablePresenter fakeInteractable)
                yield return fakeInteractable.ClickedMainButton();
            else if (TestsType == TestsType.Integration)
            {
                MainButtonComponent _mainButtonComponent = SceneContainer.Resolve<MainButtonComponent>();
                float startTime = Time.realtimeSinceStartup;
                while (Time.realtimeSinceStartup - startTime < TIMEOUT && !_mainButtonComponent.GetPrivateMember<bool>("IsActivated")) yield return null;

                if (_mainButtonComponent.GetPrivateMember<bool>("IsActivated")) _mainButtonComponent.OnMouseUpAsButton();
                else throw new TimeoutException("Main Button Not become clickable");

                yield return DotweenExtension.WaitForAnimationsComplete().AsCoroutine();
            }
        }

        protected IEnumerator ClickedTokenButton()
        {
            if (_interactablePresenter is FakeInteractablePresenter fakeInteractable)
                yield return fakeInteractable.ClickedTokenButton();
            else if (TestsType == TestsType.Integration)
            {
                TokensPileComponent tokensPileComponent = SceneContainer.Resolve<TokensPileComponent>();
                float startTime = Time.realtimeSinceStartup;

                while (Time.realtimeSinceStartup - startTime < TIMEOUT && !tokensPileComponent.GetPrivateMember<bool>("_isClickable")) yield return null;

                if (tokensPileComponent.GetPrivateMember<bool>("_isClickable")) tokensPileComponent.OnMouseUpAsButton();
                else throw new TimeoutException($"Tokenpile Not become clickable");
                yield return DotweenExtension.WaitForAnimationsComplete().AsCoroutine();
            }
        }

        protected IEnumerator ClickedUndoButton()
        {
            if (_interactablePresenter is FakeInteractablePresenter fakeInteractable)
                yield return fakeInteractable.ClickedUndoButton();
            else if (TestsType == TestsType.Integration)
            {
                UndoGameActionButton _undoGameActionButton = SceneContainer.Resolve<UndoGameActionButton>();
                while (!_undoGameActionButton.GetPrivateMember<bool>("_isPlayable")) yield return null;

                if (_undoGameActionButton.GetPrivateMember<bool>("_isPlayable")) _undoGameActionButton.OnPointerClick(null);
                else throw new TimeoutException("Main Button Not become clickable");

                yield return DotweenExtension.WaitForAnimationsComplete().AsCoroutine();
            }
        }

        /*******************************************************************/
        protected IEnumerator AlwaysHistoryPanelClick(ShowHistoryComponent _showHistoryComponent)
        {
            Button historyButton = _showHistoryComponent.GetPrivateMember<Button>("_button");
            while (!historyButton.interactable) yield return null;

            if (historyButton.interactable) historyButton.onClick.Invoke();
            else throw new TimeoutException("History Button Not become clickable");

            while (historyButton.interactable) yield return null;
            yield return AlwaysHistoryPanelClick(_showHistoryComponent);
        }

        protected IEnumerator AlwaysRegisterPanelClick(RegisterChapterComponent _registerChapterComponent)
        {
            Button registerButton = _registerChapterComponent.GetPrivateMember<Button>("_button");
            while (!registerButton.interactable) yield return null;

            if (registerButton.interactable) registerButton.onClick.Invoke();
            else throw new TimeoutException("Register Button Not become clickable");

            while (registerButton.interactable) yield return null;
            yield return AlwaysRegisterPanelClick(_registerChapterComponent);
        }

        /*******************************************************************/

        protected Card BuilCard(string cardCode)
        {
            Card bulletProof = SceneContainer.Resolve<CardLoaderUseCase>().Execute("01594");
            if (TestsType != TestsType.Unit)
                SceneContainer.TryResolve<CardViewGeneratorComponent>()?.BuildCardView(bulletProof);
            return bulletProof;
        }
    }
}