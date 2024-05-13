using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using TMPro;
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
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _value;
        [Inject] private readonly CardViewsManager _cardViewsManager;

        public Card Card { get; private set; }
        public ChallengeToken Token { get; private set; }

        /*******************************************************************/
        public Tween SetToken(ChallengeToken challengeToken, Sprite tokenSprite, Investigator investigator)
        {
            if (Card != null || Token != null) return DOTween.Sequence();
            Token = challengeToken;
            _cardImage.sprite = tokenSprite;
            _value.text = challengeToken.Value(investigator).ToString();
            transform.localScale = Vector3.zero;
            return transform.DOScale(1, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutBack)
              .OnStart(() => gameObject.SetActive(true));
        }

        public Tween SetCard(Card card, int value)
        {
            if (Card != null || Token != null) return DOTween.Sequence();
            _ = _cardImage.LoadCardSprite(card.Info.Code);
            Card = card;
            _cardView = _cardViewsManager.GetCardView(card);
            _value.text = value.ToString();
            transform.localScale = Vector3.zero;
            return transform.DOScale(1, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutBack)
                .OnStart(() => gameObject.SetActive(true));
        }

        public void Disable()
        {
            if (Card == null && Token == null) return;
            Card = null;
            Token = null;
            gameObject.SetActive(false);
            transform.SetAsLastSibling();
        }

        /*******************************************************************/
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(1.1f, ViewValues.FAST_TIME_ANIMATION);
            _cardView.CardSensor.OnMouseEnter();
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(1f, ViewValues.FAST_TIME_ANIMATION);
            _cardView.CardSensor.OnMouseExit();
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            _cardView.CardSensor.OnMouseUpAsButton();
        }
    }
}