using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ChallengeComponent : MonoBehaviour
    {
        private Vector3 initialScale;
        private Vector3 returnPosition;
        private ChallengePhaseGameAction currentChallenge;

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
        [SerializeField, Required, ChildGameObjectsOnly] private Image _token;
        [SerializeField, Required, ChildGameObjectsOnly] private Button _cancelButton;
        [SerializeField, Required, AssetsOnly] private ChallengeTokensManager _tokensManager;
        [SerializeField, Required, SceneObjectsOnly] private UndoGameActionButton _undoGameActionButton;

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
        public async Task Show(ChallengePhaseGameAction challenge, Transform worldObject = null)
        {
            if (currentChallenge != null) return;
            currentChallenge = challenge;
            transform.localScale = Vector3.zero;
            returnPosition = transform.position = (worldObject == null) ?
                _outPosition.transform.position :
                RectTransformUtility.WorldToScreenPoint(Camera.main, worldObject.transform.TransformPoint(Vector3.zero));
            await ShowAnimation().AsyncWaitForCompletion();
        }

        public async Task Hide()
        {
            await HideAnimation(returnPosition).AsyncWaitForCompletion();
            _token.gameObject.SetActive(false);
            currentChallenge = null;
        }

        public void UpdateInfo()
        {
            _skillChallengeController.SetSkill(currentChallenge.ChallengeType);
            _investigatorCardController.SetCard(currentChallenge.ActiveInvestigator.InvestigatorCard, currentChallenge.TotalChallengeValue);
            _challengeCardController.SetCard(currentChallenge.CardToChallenge, currentChallenge.DifficultValue);
            _commitCardController.ShowAll(currentChallenge.CommitsCards, currentChallenge.ChallengeType);
            _challengeName.text = currentChallenge.ChallengeName;
            _sceneTokenController.UpdateValues();
            _tokenLeftController.Refresh();
            _challengeMeterComponent.Show(currentChallenge);
            UpdateResult(currentChallenge.IsSuccessful);
        }

        private void UpdateResult(bool? isSuccessful)
        {
            if (!isSuccessful.HasValue) return;
            bool isSuccess = isSuccessful.Value;
            _result.text = isSuccess ? "Success" : "Fail";
            _result.color = isSuccess ? Color.green : Color.red;
        }

        private Sequence ShowAnimation() => DOTween.Sequence()
                .Join(transform.DOMove(_showPosition.position, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutBack, 1.1f))
                .Join(transform.DOScale(initialScale, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutBack, 1.1f));

        private Sequence HideAnimation(Vector3 returnPosition) => DOTween.Sequence()
                .Join(transform.DOMove(returnPosition, ViewValues.DEFAULT_TIME_ANIMATION))
                .Join(transform.DOScale(Vector3.zero, ViewValues.DEFAULT_TIME_ANIMATION))
                .SetEase(Ease.InOutCubic);

        private void Clicked() => _undoGameActionButton.OnPointerClick(null);

        public void SetToken(ChallengeToken token)
        {
            _token.gameObject.SetActive(true);
            _token.sprite = _tokensManager.GetToken(token.TokenType).Image;
        }

        public void ActivationCancelButton(bool isActivate)
        {
            _cancelButton.interactable = isActivate;
            _cancelButton.gameObject.SetActive(isActivate);
        }
    }
}

