using System.Threading.Tasks;
using MythosAndHorrors.GameRules;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class PresenterHub : IPresenterAnimation
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardMoverPresenter _cardMoverPresenter;
        [Inject] private readonly CreatureAttackPresenter _creatureAttackPresenter;
        [Inject] private readonly ChallengePresenter _challengePresenter;
        [Inject] private readonly PhaseChangePresenter _changePhasePresenter;
        [Inject] private readonly ChangePositionPresenter _chagePositionPresenter;
        [Inject] private readonly ShowCardPresenter _showCardPresenter;
        [Inject] private readonly EliminateInvestigatorPresenter eliminateInvestigatorPresenter;
        [Inject] private readonly FinalizePresenter _finalizePresenter;
        [Inject] private readonly PlayEffectPresenter _playEffectPresenter;
        [Inject] private readonly ShowHistoryPresenter _showHistoryPresenter;
        [Inject] private readonly ShufflePresenter _shufllePresenter;
        [Inject] private readonly StateUpdatePresenter _updateStatesPresenters;
        [Inject] private readonly StatUpdatePresenter _statsPresenter;
        [Inject] private readonly CardsDescriptionPresenter _cardsDescriptionPresenter;

        /*******************************************************************/
        async Task IPresenterAnimation.PlayBeforeAnimationWith(GameAction gameAction)
        {
            switch (gameAction)
            {
                case CreatureAttackGameAction creatureAttackGameAction:
                    await _creatureAttackPresenter.PlayAnimationWith(creatureAttackGameAction);
                    break;
                case ChallengePhaseGameAction challengePhaseGameAction:
                    await _challengePresenter.ShowChallenge(challengePhaseGameAction);
                    break;
                case CommitCardsChallengeGameAction:
                    await _challengePresenter.UpdateInfo();
                    break;
                case ResolveSingleChallengeTokenGameAction resolveSingleChallengeTokenGameAction:
                    await _challengePresenter.SolveTokenAnimation(resolveSingleChallengeTokenGameAction.ChallengeTokenToResolve);
                    break;
                case PhaseGameAction phaseGameAction:
                    await _changePhasePresenter.PlayAnimationWith(phaseGameAction);
                    break;
                case PlayEffectGameAction playEffectGameAction:
                    await _playEffectPresenter.PlayAnimationWith(playEffectGameAction);
                    break;
                case ShowHistoryGameAction showHistoryGameAction:
                    await _showHistoryPresenter.PlayAnimationWith(showHistoryGameAction);
                    break;
                default:
                    await Task.CompletedTask;
                    break;
            }
        }

        async Task IPresenterAnimation.PlayAfterAnimationWith(GameAction gameAction)
        {
            switch (gameAction)
            {
                case MoveCardsGameAction moveCardsGameAction:
                    _ = _cardsDescriptionPresenter.RefreshDescriptions();
                    await _cardMoverPresenter.PlayAnimationWith(moveCardsGameAction);
                    break;
                case ResultChallengeGameAction:
                    await _challengePresenter.UpdateInfo();
                    break;
                case FinishChallengeControlGameAction:
                    await _challengePresenter.FinalizeChallenge();
                    break;
                case ChallengePhaseGameAction challengePhaseGameAction:
                    await _changePhasePresenter.PlayAnimationWith(_gameActionsProvider.GetRealCurrentPhase() ?? challengePhaseGameAction);
                    break;
                case RevealChallengeTokenGameAction revealChallengeTokenGameAction:
                    await _challengePresenter.DropToken(revealChallengeTokenGameAction.ChallengeTokenRevealed);
                    break;
                case RestoreChallengeTokenGameAction restoreChallengeTokenGameAction:
                    await _challengePresenter.RestoreToken(restoreChallengeTokenGameAction.ChallengeTokenToRestore);
                    break;
                case ChangeCardPositionGameAction changeCardPositionGameAction:
                    await _chagePositionPresenter.PlayAnimationWith(changeCardPositionGameAction);
                    break;
                case ShowCardGameAction showCardsGameAction:
                    await _showCardPresenter.PlayAnimationWith(showCardsGameAction);
                    break;
                case EliminateInvestigatorGameAction eliminateInvestigatorGameAction:
                    await eliminateInvestigatorPresenter.PlayAnimationWith(eliminateInvestigatorGameAction);
                    break;
                case FinalizeGameAction finalizeGameAction:
                    await _finalizePresenter.PlayAnimationWith(finalizeGameAction);
                    break;
                case ShuffleGameAction shuffleGameAction:
                    await _shufllePresenter.PlayAnimationWith(shuffleGameAction);
                    break;
                case UpdateStatesGameAction updateStatesGameAction:
                    await _updateStatesPresenters.PlayAnimationWith(updateStatesGameAction);
                    break;
                case UpdateStatGameAction updateStatGameAction:
                    await _statsPresenter.PlayAnimationWith(updateStatGameAction);
                    break;
                default:
                    await Task.CompletedTask;
                    break;
            }
        }

        async Task IPresenterAnimation.PlayUndoAnimationWith(GameAction gameAction)
        {
            switch (gameAction)
            {
                case MoveCardsGameAction moveCardsGameAction:
                    await _cardMoverPresenter.PlayAnimationWith(moveCardsGameAction);
                    break;
                case ChallengePhaseGameAction:
                    await _challengePresenter.HideChallenge();
                    break;
                case ChangeCardPositionGameAction changeCardPositionGameAction:
                    await _chagePositionPresenter.PlayAnimationWith(changeCardPositionGameAction);
                    break;
                case EliminateInvestigatorGameAction eliminateInvestigatorGameAction:
                    await eliminateInvestigatorPresenter.PlayAnimationWith(eliminateInvestigatorGameAction);
                    break;
                case PhaseGameAction phaseGameAction:
                    await _changePhasePresenter.PlayAnimationWith(phaseGameAction);
                    break;
                case ShuffleGameAction shuffleGameAction:
                    await _shufllePresenter.PlayAnimationWith(shuffleGameAction);
                    break;
                case UpdateStatesGameAction updateStatesGameAction:
                    await _updateStatesPresenters.PlayAnimationWith(updateStatesGameAction);
                    break;
                case UpdateStatGameAction updateStatGameAction:
                    await _statsPresenter.PlayAnimationWith(updateStatGameAction);
                    break;
                default:
                    await Task.CompletedTask;
                    break;
            }
        }
    }
}
