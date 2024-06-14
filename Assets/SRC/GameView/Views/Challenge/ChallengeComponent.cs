using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ChallengeComponent : MonoBehaviour
    {
        private Vector3 initialScale;
        private Vector3 returnPosition;

        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [SerializeField, Required, SceneObjectsOnly] private Transform _showPosition;
        [SerializeField, Required, SceneObjectsOnly] private Transform _outPosition;
        [SerializeField, Required, ChildGameObjectsOnly] private SkillChallengeController _skillChallengeController;
        [SerializeField, Required, ChildGameObjectsOnly] private CardChallengeView _investigatorCardController;
        [SerializeField, Required, ChildGameObjectsOnly] private CardChallengeView _challengeCardController;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _challengeName;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _result;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _totalChallenge;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _difficult;
        [SerializeField, Required, ChildGameObjectsOnly] private SceneTokensController _sceneTokenController;
        [SerializeField, Required, ChildGameObjectsOnly] private TokenLeftController _tokenLeftController;
        [SerializeField, Required, ChildGameObjectsOnly] private CommitCardsController _commitCardController;
        [SerializeField, Required, ChildGameObjectsOnly] private ChallengeMeterComponent _challengeMeterComponent;
        [SerializeField, Required, AssetsOnly] private ChallengeTokensManager _tokensManager;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Initialized by Injection")]
        private void Init()
        {
            initialScale = transform.localScale;
            transform.position = _outPosition.position;
        }

        /*******************************************************************/
        public Tween UpdateInfo()
        {
            ChallengePhaseGameAction ChallengePhaseGameAction = _gameActionsProvider.CurrentChallenge ?? throw new NullReferenceException("ChallengePhaseGameAction is null");
            _investigatorCardController.SetCard(ChallengePhaseGameAction.ActiveInvestigator.InvestigatorCard, ChallengePhaseGameAction.Stat.Value);
            _challengeCardController.SetCard(ChallengePhaseGameAction.CardToChallenge, ChallengePhaseGameAction.DifficultValue);
            _commitCardController.ShowAll(ChallengePhaseGameAction.CurrentCommitsCards, ChallengePhaseGameAction.ChallengeType);
            _challengeName.text = ChallengePhaseGameAction.ChallengeName;
            _sceneTokenController.UpdateValues(ChallengePhaseGameAction.ActiveInvestigator);
            _tokenLeftController.Refresh();
            _challengeMeterComponent.Show(ChallengePhaseGameAction);
            _totalChallenge.text = ChallengePhaseGameAction.CurrentTotalChallengeValue.ToString();
            _difficult.text = ChallengePhaseGameAction.DifficultValue.ToString();
            return UpdateResult(ChallengePhaseGameAction);
        }

        public void SetToken(ChallengeToken token, Investigator investigator) => _commitCardController.ShowToken(token, investigator);

        public void RestoreToken(ChallengeToken token) => _commitCardController.RestoreToken(token);

        public async Task Show(Transform worldObject)
        {
            transform.localScale = Vector3.zero;
            returnPosition = transform.position = (worldObject == null) ?
                _outPosition.transform.position :
                RectTransformUtility.WorldToScreenPoint(Camera.main, worldObject.transform.TransformPoint(Vector3.zero));
            await ShowAnimation().AsyncWaitForCompletion();
        }

        public async Task Hide()
        {
            await HideAnimation(returnPosition).AsyncWaitForCompletion();
            _investigatorCardController.Disable();
            _challengeCardController.Disable();
            _sceneTokenController.Disable();
            _commitCardController.ClearAll();
        }

        private Tween UpdateResult(ChallengePhaseGameAction ChallengePhaseGameAction)
        {
            if (!ChallengePhaseGameAction.IsSuccessful.HasValue)
            {
                _result.transform.DOScale(0, 0).SetEase(Ease.InBack);
                _result.text = string.Empty;
                _result.color = Color.white;
                _skillChallengeController.SetSkill(ChallengePhaseGameAction.ChallengeType);
            }
            else
            {
                _result.text = ChallengePhaseGameAction.IsSuccessful.Value ? "Success" : "Fail";
                _result.color = ChallengePhaseGameAction.IsSuccessful.Value ? Color.green : Color.red;
                return DOTween.Sequence().Append(_skillChallengeController.ShutDown())
                    .Append(_result.transform.DOScale(1, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutBack));
            }
            return DOTween.Sequence();
        }

        private Sequence ShowAnimation() => DOTween.Sequence()
                .Join(transform.DOMove(_showPosition.position, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutBack, 1.1f))
                .Join(transform.DOScale(initialScale, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutBack, 1.1f));

        private Sequence HideAnimation(Vector3 returnPosition) => DOTween.Sequence()
                .Insert(ViewValues.SLOW_TIME_ANIMATION, transform.DOMove(returnPosition, ViewValues.DEFAULT_TIME_ANIMATION))
                .Join(transform.DOScale(Vector3.zero, ViewValues.DEFAULT_TIME_ANIMATION))
                .SetEase(Ease.InOutCubic);
    }
}

