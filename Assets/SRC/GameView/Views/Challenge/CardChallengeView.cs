using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Threading.Tasks;
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

        /*******************************************************************/
        public async Task SetCard(Card card, int value)
        {
            if (Card != card) await _cardImage.LoadCardSprite(card.Info.Code);
            Card = card;
            _cardView = _cardViewsManager.GetCardView(card);
            _value.text = value.ToString();
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            Card = null;
            gameObject.SetActive(false);
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