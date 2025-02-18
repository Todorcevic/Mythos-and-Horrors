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
        [Inject] private readonly TextsManager _textsProvider;
        [Inject] private readonly AudioComponent _audioComponent;
        [SerializeField, Required, SceneObjectsOnly] private Transform _showPosition;
        [SerializeField, Required, SceneObjectsOnly] private Transform _outPosition;
        [SerializeField, Required, ChildGameObjectsOnly] private CardChallengeView _investigatorCardController;
        [SerializeField, Required, ChildGameObjectsOnly] private CardChallengeView _challengeCardController;
        [SerializeField, Required, ChildGameObjectsOnly] private CommitCardsController _commitCardController;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _challengeName;
        [SerializeField, Required, ChildGameObjectsOnly] private ChallengeTokensInfoController _challengeTokensInfoController;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _resultMessage;
        [SerializeField, Required, ChildGameObjectsOnly] private ChallengeStatsController _totalChallengeStatController;
        [SerializeField, Required, ChildGameObjectsOnly] private ChallengeStatsController _difficultStatController;
        [SerializeField, Required, ChildGameObjectsOnly] private ResultInfoComponent _resultInfoComponent;
        [SerializeField, Required, AssetsOnly] private AudioClip _showAudio;
        [SerializeField, Required, AssetsOnly] private AudioClip _hideAudio;

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
            _challengeName.text = _textsProvider.GetLocalizableText(ChallengePhaseGameAction.ChallengeName);
            _challengeTokensInfoController.ShowRevealedTokens(ChallengePhaseGameAction.ActiveInvestigator);
            _totalChallengeStatController.SetStat(ChallengePhaseGameAction.ChallengeType, ChallengePhaseGameAction.CurrentTotalChallengeValue);
            _difficultStatController.SetStat(ChallengePhaseGameAction.ChallengeType, ChallengePhaseGameAction.DifficultValue);
            return _resultInfoComponent.Show(ChallengePhaseGameAction);
        }

        /*******************************************************************/
        public bool IsShowed { get; private set; }
        public Sequence Show(Transform worldObject, Investigator investigator)
        {
            IsShowed = true;
            _challengeTokensInfoController.Init(investigator);
            transform.localScale = Vector3.zero;
            returnPosition = transform.position = (worldObject == null) ? _outPosition.transform.position :
                RectTransformUtility.WorldToScreenPoint(Camera.main, worldObject.transform.TransformPoint(Vector3.zero));
            return ShowAnimation();
        }

        public Sequence Hide()
        {
            IsShowed = false;
            return HideAnimation(returnPosition).OnComplete(Finishing);

            /*******************************************************************/
            void Finishing()
            {
                _investigatorCardController.Disable();
                _challengeCardController.Disable();
                _commitCardController.ClearAll();
            }
        }

        private Sequence ShowAnimation() => DOTween.Sequence()
                .OnStart(() => _audioComponent.PlayAudio(_showAudio))
                .Join(transform.DOMove(_showPosition.position, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutBack, 1.1f))
                .Join(transform.DOScale(initialScale, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutBack, 1.1f));

        private Sequence HideAnimation(Vector3 returnPosition) => DOTween.Sequence()
                .OnStart(() => _audioComponent.PlayAudio(_hideAudio))
                .Join(transform.DOMove(returnPosition, ViewValues.DEFAULT_TIME_ANIMATION))
                .Join(transform.DOScale(Vector3.zero, ViewValues.DEFAULT_TIME_ANIMATION))
                .SetEase(Ease.InOutCubic);
    }
}