using System.Threading.Tasks;
using MythosAndHorrors.GameRules;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class MainPresenter : IPresenterAnimation
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly IPresenter<MoveCardsGameAction> _cardMoverPresenter;
        [Inject] private readonly IPresenter<CreatureAttackGameAction> _creatureAttackPresenter;
        [Inject] private readonly IPresenter<ChallengePhaseGameAction> _challengePresenter;
        [Inject] private readonly IPresenter<CommitCardsChallengeGameAction> _commitCardsChallengeGameAction;
        [Inject] private readonly IPresenter<RevealChallengeTokenGameAction> _revealTokenPresenter;
        [Inject] private readonly IPresenter<ResolveSingleChallengeTokenGameAction> _solveTokenPresenter;
        [Inject] private readonly IPresenter<RestoreChallengeTokenGameAction> _restoreChallengeTokenPresenter;
        [Inject] private readonly IPresenter<PhaseGameAction> _changePhasePresenter;
        [Inject] private readonly IPresenter<ChangeCardPositionGameAction> _chagePositionPresenter;
        [Inject] private readonly IPresenter<ShowCardsGameAction> _drawPresenter;
        [Inject] private readonly IPresenter<EliminateInvestigatorGameAction> eliminateInvestigatorPresenter;
        [Inject] private readonly IPresenter<FinalizeGameAction> _finalizePresenter;
        [Inject] private readonly IPresenter<PlayEffectGameAction> _playEffectPresenter;
        [Inject] private readonly IPresenter<ShowHistoryGameAction> _showHistoryPresenter;
        [Inject] private readonly IPresenter<ShuffleGameAction> _shufllePresenter;
        [Inject] private readonly IPresenter<UpdateStatesGameAction> _updateStatesPresenters;
        [Inject] private readonly IPresenter<UpdateStatGameAction> _statsPresenter;

        /*******************************************************************/
        async Task IPresenterAnimation.PlayBeforeAnimationWith(GameAction gameAction)
        {
            switch (gameAction)
            {
                case CreatureAttackGameAction creatureAttackGameAction:
                    await _creatureAttackPresenter.PlayAnimationWith(creatureAttackGameAction);
                    break;
                case ChallengePhaseGameAction challengePhaseGameAction:
                    await _challengePresenter.PlayAnimationWith(challengePhaseGameAction);
                    break;
                case CommitCardsChallengeGameAction commitCardsChallengeGameAction:
                    await _commitCardsChallengeGameAction.PlayAnimationWith(commitCardsChallengeGameAction);
                    break;
                case ResolveSingleChallengeTokenGameAction resolveSingleChallengeTokenGameAction:
                    await _solveTokenPresenter.PlayAnimationWith(resolveSingleChallengeTokenGameAction);
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
                    await _cardMoverPresenter.PlayAnimationWith(moveCardsGameAction);
                    break;
                case ResultChallengeGameAction resultChallengeGameAction:
                    await _challengePresenter.PlayAnimationWith(resultChallengeGameAction.ChallengePhaseGameAction);
                    break;
                case ChallengePhaseGameAction challengePhaseGameAction:
                    await _changePhasePresenter.PlayAnimationWith(_gameActionsProvider.GetRealCurrentPhase() ?? challengePhaseGameAction);
                    break;
                case RevealChallengeTokenGameAction revealChallengeTokenGameAction:
                    await _revealTokenPresenter.PlayAnimationWith(revealChallengeTokenGameAction);
                    break;
                case RestoreChallengeTokenGameAction restoreChallengeTokenGameAction:
                    await _restoreChallengeTokenPresenter.PlayAnimationWith(restoreChallengeTokenGameAction);
                    break;
                case ChangeCardPositionGameAction changeCardPositionGameAction:
                    await _chagePositionPresenter.PlayAnimationWith(changeCardPositionGameAction);
                    break;
                case ShowCardsGameAction showCardsGameAction:
                    await _drawPresenter.PlayAnimationWith(showCardsGameAction);
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
                case ChallengePhaseGameAction challengePhaseGameAction:
                    await _challengePresenter.PlayAnimationWith(challengePhaseGameAction);
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
