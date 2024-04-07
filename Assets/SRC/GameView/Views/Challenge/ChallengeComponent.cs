using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ChallengeComponent : MonoBehaviour, IPlayable
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
        [SerializeField, Required, ChildGameObjectsOnly] private SceneTokensController _sceneTokenController;
        [SerializeField, Required, ChildGameObjectsOnly] private TokenLeftController _tokenLeftController;
        [SerializeField, Required, ChildGameObjectsOnly] private CommitCardsController _commitCardController;
        [SerializeField, Required, ChildGameObjectsOnly] private ChallengeMeterComponent _challengeMeterComponent;
        [SerializeField, Required, ChildGameObjectsOnly] private Button _cancelButton;
        [SerializeField, Required, AssetsOnly] private ChallengeTokensManager _tokensManager;
        [SerializeField, Required, SceneObjectsOnly] private UndoGameActionButton _undoGameActionButton;

        /*******************************************************************/
        private Effect UndoEffect => _gameActionsProvider.CurrentInteractable?.UndoEffect;
        IEnumerable<Effect> IPlayable.EffectsSelected => UndoEffect == null ? Enumerable.Empty<Effect>() : new[] { UndoEffect };
        void IPlayable.ActivateToClick() => ActivationCancelButton(true);
        void IPlayable.DeactivateToClick() => ActivationCancelButton(false);

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Initialized by Injection")]
        private void Init()
        {
            initialScale = transform.localScale;
            transform.position = _outPosition.position;
            _cancelButton.onClick.AddListener(Clicked);
        }

        /*******************************************************************/
        public Tween UpdateInfo(ChallengePhaseGameAction ChallengePhaseGameAction)
        {
            _skillChallengeController.SetSkill(ChallengePhaseGameAction.ChallengeType);
            _investigatorCardController.SetCard(ChallengePhaseGameAction.ActiveInvestigator.InvestigatorCard, ChallengePhaseGameAction.TotalChallengeValue);
            _challengeCardController.SetCard(ChallengePhaseGameAction.CardToChallenge, ChallengePhaseGameAction.DifficultValue);
            _commitCardController.ShowAll(ChallengePhaseGameAction.CommitsCards, ChallengePhaseGameAction.ChallengeType);
            _challengeName.text = ChallengePhaseGameAction.ChallengeName;
            _sceneTokenController.UpdateValues();
            _tokenLeftController.Refresh();
            _challengeMeterComponent.Show(ChallengePhaseGameAction);
            return UpdateResult(ChallengePhaseGameAction.IsSuccessful);
        }

        public void SetToken(ChallengeToken token)
        {
            _commitCardController.ShowToken(token);
        }

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
            _commitCardController.ClearAll();
        }

        private Tween UpdateResult(bool? isSuccessful)
        {
            if (!isSuccessful.HasValue)
            {
                _result.DOFade(0, 0);
                _result.text = string.Empty;
                _result.color = Color.white;
            }
            else
            {
                _result.text = isSuccessful.Value ? "Success" : "Fail";
                _result.color = isSuccessful.Value ? Color.green : Color.red;
                return _result.DOFade(1, ViewValues.DEFAULT_TIME_ANIMATION);
            }
            return DOTween.Sequence();
        }

        private Sequence ShowAnimation() => DOTween.Sequence()
                .Join(transform.DOMove(_showPosition.position, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutBack, 1.1f))
                .Join(transform.DOScale(initialScale, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutBack, 1.1f));

        private Sequence HideAnimation(Vector3 returnPosition) => DOTween.Sequence()
                .Join(transform.DOMove(returnPosition, ViewValues.DEFAULT_TIME_ANIMATION))
                .Join(transform.DOScale(Vector3.zero, ViewValues.DEFAULT_TIME_ANIMATION))
                .SetEase(Ease.InOutCubic);

        private void Clicked() => _undoGameActionButton.OnPointerClick(null);

        private void ActivationCancelButton(bool isActivate)
        {
            _cancelButton.interactable = isActivate;
            _cancelButton.gameObject.SetActive(isActivate);
        }
    }
}

