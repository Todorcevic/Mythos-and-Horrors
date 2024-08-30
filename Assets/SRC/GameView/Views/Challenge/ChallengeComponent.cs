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
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;
        [Inject] private readonly TextsProvider _textsProvider;
        [SerializeField, Required, SceneObjectsOnly] private Transform _showPosition;
        [SerializeField, Required, SceneObjectsOnly] private Transform _outPosition;
        [SerializeField, Required, ChildGameObjectsOnly] private CardChallengeView _investigatorCardController;
        [SerializeField, Required, ChildGameObjectsOnly] private CardChallengeView _challengeCardController;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _challengeName;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _message;
        [SerializeField, Required, ChildGameObjectsOnly] private ChallengeStatsController _totalChallengeStatController;
        [SerializeField, Required, ChildGameObjectsOnly] private ChallengeStatsController _difficultStatController;
        [SerializeField, Required, ChildGameObjectsOnly] private TokensRowController _tokenRowTopController;
        [SerializeField, Required, ChildGameObjectsOnly] private TokensRowController _tokenRowBottomController;
        [SerializeField, Required, ChildGameObjectsOnly] private CommitCardsController _commitCardController;
        [SerializeField, Required, ChildGameObjectsOnly] private ChallengeMeterComponent _challengeMeterComponent;

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
            _investigatorCardController.SetCard(ChallengePhaseGameAction.ActiveInvestigator.InvestigatorCard, ChallengePhaseGameAction.ChallengeType, ChallengePhaseGameAction.Stat.Value);
            _challengeCardController.SetCard(ChallengePhaseGameAction.CardToChallenge, ChallengePhaseGameAction.ChallengeType, ChallengePhaseGameAction.DifficultValue);
            _commitCardController.ShowAll(ChallengePhaseGameAction.CurrentCommitsCards, ChallengePhaseGameAction.ChallengeType);
            _challengeName.text = ChallengePhaseGameAction.ChallengeName;
            _tokenRowTopController.SetWith(ChallengePhaseGameAction.ActiveInvestigator, _challengeTokensProvider.BasicChallengeTokensInBag);
            _tokenRowBottomController.SetWith(ChallengePhaseGameAction.ActiveInvestigator, _challengeTokensProvider.SpecialChallengeTokensInBag);
            _challengeMeterComponent.Show(ChallengePhaseGameAction);
            _totalChallengeStatController.SetStat(ChallengePhaseGameAction.ChallengeType, ChallengePhaseGameAction.CurrentTotalChallengeValue);
            _difficultStatController.SetStat(ChallengePhaseGameAction.ChallengeType, ChallengePhaseGameAction.DifficultValue);
            return UpdateResult(ChallengePhaseGameAction);
        }

        public void SetToken(ChallengeToken token, Investigator investigator)
        {
            //_commitCardController.ShowToken(token, investigator);
        }

        public void RestoreToken(ChallengeToken token)
        {
            //_commitCardController.RestoreToken(token); 
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
            _investigatorCardController.Disable();
            _challengeCardController.Disable();
            _commitCardController.ClearAll();
        }

        private Tween UpdateResult(ChallengePhaseGameAction ChallengePhaseGameAction)
        {
            bool? isSuccessful = ChallengePhaseGameAction.ResultChallenge?.IsSuccessful;
            if (!isSuccessful.HasValue) return DOTween.Sequence();

            _message.transform.DOScale(0, 0).SetEase(Ease.InBack);
            _message.text = isSuccessful.Value ? _textsProvider.GetLocalizableText("Challenge_Component_Succeed") : _textsProvider.GetLocalizableText("Challenge_Component_Fail");
            _message.color = isSuccessful.Value ? Color.green : Color.red;
            return _message.transform.DOScale(1, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutBack);
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

