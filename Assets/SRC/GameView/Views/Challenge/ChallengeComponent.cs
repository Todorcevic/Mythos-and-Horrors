using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
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

        [SerializeField, Required, SceneObjectsOnly] private Transform _showPosition;
        [SerializeField, Required, SceneObjectsOnly] private Transform _outPosition;
        [SerializeField, Required, ChildGameObjectsOnly] private SkillChallengeController _skillChallengeController;
        [SerializeField, Required, ChildGameObjectsOnly] private CardChallengeController _investigatorCardController;
        [SerializeField, Required, ChildGameObjectsOnly] private CardChallengeController _challengeCardController;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _statValue;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _difficultValue;
        [SerializeField, Required, ChildGameObjectsOnly] private SceneTokensController _sceneTokenController;
        [SerializeField, Required, ChildGameObjectsOnly] private Image _token;
        [SerializeField, Required, ChildGameObjectsOnly] private Button _cancelButton;

        /******************** MANAGER **************************/
        [SerializeField, Required, AssetsOnly] private ChallengeTokenSO _voidToken;
        [SerializeField, Required, AssetsOnly] private List<ChallengeTokenSO> _tokens;

        public ChallengeTokenSO GetToken(ChallengeTokenType tokenType) =>
            _tokens.Find(token => token.TokenType == tokenType) ?? _voidToken;

        /*******************************************************************/
        [Inject]
        private void Init()
        {
            initialScale = transform.localScale;
            _cancelButton.onClick.AddListener(Clicked);
            _sceneTokenController.SetTokens();
        }

        /*******************************************************************/
        public async Task Show(ChallengePhaseGameAction challenge, Transform worldObject = null)
        {
            transform.localScale = Vector3.zero;
            returnPosition = transform.position = (worldObject == null) ? _outPosition.transform.position : RectTransformUtility.WorldToScreenPoint(Camera.main, worldObject.transform.TransformPoint(Vector3.zero));

            await UpdateInfo(challenge);
            await ShowAnimation().AsyncWaitForCompletion();
        }

        public async Task Hide()
        {
            await HideAnimation(returnPosition).AsyncWaitForCompletion();
        }

        private async Task UpdateInfo(ChallengePhaseGameAction challenge)
        {
            _skillChallengeController.SetSkill(challenge.ChallengeType);
            await _investigatorCardController.SetCard(challenge.ActiveInvestigator.InvestigatorCard);
            await _challengeCardController.SetCard(challenge.CardToChallenge);
            _statValue.text = challenge.Stat.Value.ToString();
            _difficultValue.text = challenge.DifficultValue.ToString();
            _sceneTokenController.UpdateValues();
        }

        private Sequence ShowAnimation() => DOTween.Sequence()
                .Join(transform.DOMove(_showPosition.position, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutBack, 1.1f))
                .Join(transform.DOScale(initialScale, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutBack, 1.1f));

        private Sequence HideAnimation(Vector3 returnPosition) => DOTween.Sequence()

                .Join(transform.DOMove(returnPosition, ViewValues.DEFAULT_TIME_ANIMATION))
                .Join(transform.DOScale(Vector3.zero, ViewValues.DEFAULT_TIME_ANIMATION))
                .SetEase(Ease.InOutCubic);


        private void Clicked()
        {

        }


        public void SetToken(ChallengeToken token)
        {
            _token.sprite = GetToken(token.TokenType).Image;
        }
    }
}

