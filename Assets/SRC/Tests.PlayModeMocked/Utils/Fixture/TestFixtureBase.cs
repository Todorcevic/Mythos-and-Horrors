using MythosAndHorrors.EditMode.Tests;
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
        [Inject] protected readonly ShowHistoryComponent _showHistoryComponent;
        [Inject] protected readonly RegisterChapterComponent _registerChapterComponent;
        [Inject] private readonly IInteractablePresenter _interactablePresenter;

        private static string currentSceneName;
        protected abstract string JSON_SAVE_DATA_PATH { get; }
        protected virtual string SCENE_NAME => "GamePlayCORE1";
        protected virtual TestsType TestsType => TestsType.Debug;

        /*******************************************************************/
        [UnitySetUp]
        public override IEnumerator SetUp()
        {
            if (currentSceneName != JSON_SAVE_DATA_PATH)
            {
                currentSceneName = JSON_SAVE_DATA_PATH;
                if (TestsType == TestsType.Debug)
                {
                    InstallerToScene();
                    yield return LoadScene(SCENE_NAME, InstallerToTests);
                }
                else
                {
                    LoadContainer();
                    _prepareGameRulesUseCase.Execute();
                }
                AlwaysHistoryPanelClick().AsTask();
                AlwaysRegisterPanelClick().AsTask();
            }
            else SceneContainer?.Inject(this);

            yield return null;
        }

        private void LoadContainer()
        {
            SceneContainer = new();
            SceneContainer.Install<InjectionService>();
            InstallerToTests();
            SceneContainer?.Inject(this);
        }

        [UnityTearDown]
        public override IEnumerator TearDown()
        {
            yield return _gameActionsProvider.Rewind().AsCoroutine().Fast();
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
            SceneContainer.Bind<PreparationSceneCORE1PlayModeAdapted>().AsSingle();
            SceneContainer.Bind<PreparationSceneCORE2PlayModeAdapted>().AsSingle();
            SceneContainer.Bind<PreparationSceneCORE3PlayModeAdapted>().AsSingle();
            if (TestsType != TestsType.Debug)
            {
                SceneContainer.BindInstance(JSON_SAVE_DATA_PATH).WhenInjectedInto<DataSaveUseCase>();
                SceneContainer.BindInstance(false).WhenInjectedInto<InitializerComponent>();
                SceneContainer.Rebind<IInteractablePresenter>().To<FakeInteractablePresenter>().AsCached();
                BindAllFakePresenters();
            }

            static void BindAllFakePresenters()
            {
                IEnumerable<Type> gameActionTypes = typeof(GameAction).Assembly.GetTypes().Where(type => type.IsClass);

                foreach (Type type in gameActionTypes)
                {
                    BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
                    FieldInfo[] campos = type.GetFields(flags);

                    foreach (FieldInfo campo in campos)
                    {
                        if (campo.FieldType.IsGenericType &&
                            campo.FieldType.GetGenericTypeDefinition() == typeof(IPresenter<>) && campo.FieldType.GetGenericArguments()[0] == type)
                        {
                            Type genericToBind = typeof(FakePresenter<>).MakeGenericType(type);
                            SceneContainer.Rebind(campo.FieldType).To(genericToBind).AsCached();
                        }
                    }
                }
            }
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

        /*******************************************************************/
        protected IEnumerator ClickedIn(Card card)
        {
            if (_interactablePresenter is FakeInteractablePresenter fakeInteractable)
                yield return fakeInteractable.ClickedIn(card);
        }

        protected IEnumerator ClickedMainButton()
        {
            if (_interactablePresenter is FakeInteractablePresenter fakeInteractable)
                yield return fakeInteractable.ClickedMainButton();
        }

        protected IEnumerator ClickedTokenButton()
        {
            if (_interactablePresenter is FakeInteractablePresenter fakeInteractable)
                yield return fakeInteractable.ClickedTokenButton();
        }

        protected IEnumerator ClickedUndoButton()
        {
            if (_interactablePresenter is FakeInteractablePresenter fakeInteractable)
                yield return fakeInteractable.ClickedUndoButton();
        }

        /*******************************************************************/
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
    }
}