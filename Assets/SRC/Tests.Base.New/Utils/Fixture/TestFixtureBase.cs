using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using System.Collections.Generic;
using System;
using UnityEngine.TestTools;
using Zenject;
using System.Collections;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

namespace MythosAndHorrors.PlayMode.Tests
{
    public abstract class TestFixtureBase : SceneTestLoader
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

        protected override TestsType TestsType => TestsType.Unit;

        /*******************************************************************/
        protected override void PrepareUnitTests()
        {
            base.PrepareUnitTests();
            _prepareGameRulesUseCase.Execute();
        }

        protected override IEnumerator PrepareIntegrationTests()
        {
            yield return base.PrepareIntegrationTests();
            Time.timeScale = TestsType == TestsType.Debug ? 1 : 64;
            DOTween.SetTweensCapacity(1250, 312);
            AlwaysHistoryPanelClick(SceneContainer.Resolve<ShowHistoryComponent>()).AsTask();
            AlwaysRegisterPanelClick(SceneContainer.Resolve<RegisterChapterComponent>()).AsTask();

            /*******************************************************************/
            IEnumerator AlwaysHistoryPanelClick(ShowHistoryComponent _showHistoryComponent)
            {
                Button historyButton = _showHistoryComponent.GetPrivateMember<Button>("_button");
                while (!historyButton.interactable) yield return null;

                if (historyButton.interactable) historyButton.onClick.Invoke();
                else throw new TimeoutException("History Button Not become clickable");

                while (historyButton.interactable) yield return null;
                yield return AlwaysHistoryPanelClick(_showHistoryComponent);
            }

            IEnumerator AlwaysRegisterPanelClick(RegisterChapterComponent _registerChapterComponent)
            {
                Button registerButton = _registerChapterComponent.GetPrivateMember<Button>("_button");
                while (!registerButton.interactable) yield return null;

                if (registerButton.interactable) registerButton.onClick.Invoke();
                else throw new TimeoutException("Register Button Not become clickable");

                while (registerButton.interactable) yield return null;
                yield return AlwaysRegisterPanelClick(_registerChapterComponent);
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

        protected IEnumerator ClickedClone(Card card, int position, bool isReaction = false)
        {
            if (_interactablePresenter is FakeInteractablePresenter fakeInteractable)
                yield return fakeInteractable.ClickedIn(card, position);
            else if (TestsType == TestsType.Integration)
            {
                if (!isReaction) yield return ClickedIn(card);
                MultiEffectHandler _multiEffectHandler = SceneContainer.Resolve<MultiEffectHandler>();
                float startTime = Time.realtimeSinceStartup;
                while (Time.realtimeSinceStartup - startTime < TIMEOUT && _gameActionsProvider.CurrentInteractable == null) yield return null;
                while (Time.realtimeSinceStartup - startTime < TIMEOUT && _multiEffectHandler.GetPrivateMember<List<IPlayable>>("cardViewClones") == null) yield return null;
                if (_gameActionsProvider.CurrentInteractable == null || _multiEffectHandler.GetPrivateMember<List<IPlayable>>("cardViewClones") == null)
                    throw new TimeoutException($"Clone position: {position} Not become clickable");
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

        protected bool IsClickable(Card card)
        {
            if (_interactablePresenter is FakeInteractablePresenter fakeInteractable)
                return fakeInteractable.IsClickable(card);
            else if (TestsType == TestsType.Integration)
            {
                CardViewsManager _cardViewsManager = SceneContainer.Resolve<CardViewsManager>();
                CardSensorController cardSensor = _cardViewsManager.GetCardView(card).GetPrivateMember<CardSensorController>("_cardSensor");
                return cardSensor.IsClickable;
            }
            return false;
        }

        /*******************************************************************/
        protected Card BuilCard(string cardCode)
        {
            Card cardCreated = SceneContainer.Resolve<CardLoaderUseCase>().Execute(cardCode);
            if (TestsType != TestsType.Unit)
                SceneContainer.TryResolve<CardViewGeneratorComponent>()?.BuildCardView(cardCreated);
            return cardCreated;
        }
    }
}