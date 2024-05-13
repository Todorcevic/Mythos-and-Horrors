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
        protected abstract string SCENE_NAME { get; }
        protected abstract string JSON_SAVE_DATA_PATH { get; }
        protected FakeInteractablePresenter FakeInteractablePresenter => (FakeInteractablePresenter)_interactablePresenter;
        protected static new DiContainer SceneContainer { get; private set; }

        /*******************************************************************/
        [UnitySetUp]
        public override IEnumerator SetUp()
        {
            if (currentSceneName != SCENE_NAME)
            {
                currentSceneName = SCENE_NAME;
                LoadContainer();
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
        protected void MustBeRevealedThisToken(ChallengeTokenType tokenType)
        {
            _reactionablesProvider.CreateReaction<RevealChallengeTokenGameAction>((_) => true, Reveal, isAtStart: true);

            /*******************************************************************/
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

        protected async Task<(ChallengeToken token, int value)> CaptureToken(Investigator investigator)
        {
            TaskCompletionSource<ChallengeToken> waitForReaction = new();
            _reactionablesProvider.CreateReaction<RevealChallengeTokenGameAction>((_) => true, Reveal, isAtStart: false);
            await waitForReaction.Task;
            return (waitForReaction.Task.Result, waitForReaction.Task.Result.Value(investigator));

            /*******************************************************************/
            async Task Reveal(GameAction gameAction)
            {
                if (gameAction is not RevealChallengeTokenGameAction revealChallengeTokenGameAction) return;

                waitForReaction.SetResult(revealChallengeTokenGameAction.ChallengeTokenRevealed);
                _reactionablesProvider.RemoveReaction<RevealChallengeTokenGameAction>(Reveal);
                await Task.CompletedTask;
            }
        }
    }
}