using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class CardChallengeView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private CardView _cardView;
        [SerializeField, Required, ChildGameObjectsOnly] private Image _cardImage;
        [SerializeField, Required, ChildGameObjectsOnly] private ChallengeStatsController _challengeStatsControler;
        [Inject] private readonly CardViewsManager _cardViewsManager;

        public Card Card { get; private set; }

        /*******************************************************************/
        public Tween SetCard(Card card, ChallengeType challengeType, int value)
        {
            if (Card != null) return DOTween.Sequence();
            _ = _cardImage.LoadCardSprite(card.Info.Code);
            Card = card;
            transform.SetAsLastSibling();
            _cardView = _cardViewsManager.GetCardView(card);
            _challengeStatsControler.SetStat(challengeType, value);

            if (card is CommitableCard commitableCard && commitableCard.Wild.Value > 0)
                _challengeStatsControler.SetWildStat(commitableCard.Wild.Value);

            transform.localScale = Vector3.zero;
            return transform.DOScale(1, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutBack)
                .OnStart(() => gameObject.SetActive(true));
        }

        public void Disable()
        {
            if (Card == null) return;
            Card = null;
            gameObject.SetActive(false);
            transform.SetAsLastSibling();
        }

        /*******************************************************************/
        private int realSibling;

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            realSibling = transform.GetSiblingIndex();
            transform.SetAsLastSibling();
            transform.DOScale(1.1f, ViewValues.FAST_TIME_ANIMATION);
            _cardView.CardSensor.MouseEnter();
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            transform.SetSiblingIndex(realSibling);
            transform.DOScale(1f, ViewValues.FAST_TIME_ANIMATION);
            _cardView.CardSensor.MouseExit();
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            _cardView.CardSensor.MouseUpAsButton();
        }
    }
}