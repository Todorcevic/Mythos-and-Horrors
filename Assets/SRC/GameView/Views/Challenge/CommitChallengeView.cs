using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class CommitChallengeView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private CardView _cardView;
        [SerializeField, Required, ChildGameObjectsOnly] private Image _cardImage;
        [SerializeField, Required, ChildGameObjectsOnly] private ChallengeStatsController _challengeStatsControler;
        [Inject] private readonly CardViewsManager _cardViewsManager;

        public CommitableCard CommitableCard { get; private set; }

        /*******************************************************************/
        public Tween SetCard(CommitableCard commitableCard, ChallengeType challengeType, int value)
        {
            if (CommitableCard != null) return DOTween.Sequence();
            _ = _cardImage.LoadCardSprite(commitableCard.Info.Code);
            CommitableCard = commitableCard;
            transform.SetAsLastSibling();
            _cardView = _cardViewsManager.GetCardView(commitableCard);
            _challengeStatsControler.SetStat(challengeType, value);

            if (commitableCard.Wild.Value > 0) _challengeStatsControler.SetWildStat(commitableCard.Wild.Value);

            transform.localScale = Vector3.zero;
            return transform.DOScale(1, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutBack)
                .OnStart(() => gameObject.SetActive(true));
        }

        public void Disable()
        {
            if (CommitableCard == null) return;
            CommitableCard = null;
            gameObject.SetActive(false);
            transform.SetAsLastSibling();
        }

        /*******************************************************************/
        private int realSibling;

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            realSibling = transform.GetSiblingIndex();
            transform.SetAsLastSibling();
            _cardImage.DOColor(new Color(0.8f, 0.8f, 0.8f), ViewValues.FAST_TIME_ANIMATION).SetNotWaitable();
            transform.DOScale(1.1f, ViewValues.FAST_TIME_ANIMATION).SetNotWaitable();
            _cardView.CardSensor.MouseEnter();
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            transform.SetSiblingIndex(realSibling);
            _cardImage.DOColor(Color.white, ViewValues.FAST_TIME_ANIMATION).SetNotWaitable();
            transform.DOScale(1f, ViewValues.FAST_TIME_ANIMATION).SetNotWaitable();
            _cardView.CardSensor.MouseExit();
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            _cardView.CardSensor.MouseUpAsButton();
        }
    }
}