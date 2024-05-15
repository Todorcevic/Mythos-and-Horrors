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
using UnityEngine;
using System.Drawing;

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
        protected virtual bool DEBUG_MODE => false;
        protected FakeInteractablePresenter FakeInteractablePresenter => (FakeInteractablePresenter)_interactablePresenter;
        protected static new DiContainer SceneContainer { get; private set; }

        /*******************************************************************/
        [UnitySetUp]
        public override IEnumerator SetUp()
        {
            if (currentSceneName != JSON_SAVE_DATA_PATH)
            {
                currentSceneName = JSON_SAVE_DATA_PATH;
                LoadContainer();
                if (DEBUG_MODE) InitializeTextForConsole();
                _prepareGameRulesUseCase.Execute();
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
            yield return _gameActionsProvider.Rewind().AsCoroutine();
        }

        private void InstallerToTests()
        {
            SceneContainer.BindInstance(JSON_SAVE_DATA_PATH).WhenInjectedInto<DataSaveUseCase>();
            SceneContainer.BindInstance(false).WhenInjectedInto<InitializerComponent>();
            SceneContainer.Bind<PreparationSceneCORE1>().AsSingle();
            SceneContainer.Bind<PreparationSceneCORE2>().AsSingle();
            SceneContainer.Bind<PreparationSceneCORE3>().AsSingle();
            SceneContainer.Rebind<IInteractablePresenter>().To<FakeInteractablePresenter>().AsCached();
            BindAllFakePresenters();

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

        private void InitializeTextForConsole()
        {
            ShowCurrentGameActionInConsole();
            ShowCurrentEffectInConsole();
        }

        private void ShowCurrentGameActionInConsole()
        {
            _reactionablesProvider.CreateReaction<GameAction>((_) => true, ShowMesaggeInConsole, isAtStart: true);

            /*******************************************************************/
            static async Task ShowMesaggeInConsole(GameAction action)
            {
                Debug.Log("<color=#FFA500>* GameAction: " + action.GetType().Name + "\n</color>");
                await Task.CompletedTask;
            }
        }

        private void ShowCurrentEffectInConsole()
        {
            _reactionablesProvider.CreateReaction<PlayEffectGameAction>((_) => true, ShowMesaggeInConsole, isAtStart: true);

            /*******************************************************************/
            static async Task ShowMesaggeInConsole(PlayEffectGameAction playEffectGameAction)
            {
                InteractableGameAction interactable = (InteractableGameAction)playEffectGameAction.Parent;
                Debug.Log("<color=cyan>** All Effects: \n</color>");
                foreach (Effect effect in interactable.AllEffects
                    .Append(interactable.UndoEffect).Append(interactable.MainButtonEffect))
                {
                    Debug.Log("<color=cyan>---- " + effect?.Description + "\n</color>");
                }

                Debug.Log("<color=yellow>**** EffectPressed: " + playEffectGameAction.Effect.Description + "\n</color>");
                await Task.CompletedTask;
            }
        }
    }
}