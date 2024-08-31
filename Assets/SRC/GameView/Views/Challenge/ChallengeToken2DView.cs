using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MythosAndHorrors.GameView
{
    public class ChallengeToken2DView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        //[SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _value;
        [SerializeField, Required, ChildGameObjectsOnly] private Image _image;
        private string _value;
        private string _description;
        private TextMeshProUGUI _message;

        /*******************************************************************/
        public void SetImage(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        public void SetToken(int value, string description, TextMeshProUGUI message)
        {
            _message = message;
            _description = description;
            _value = $"{(value > 0 ? "+" : "")}{value}";
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(1.2f, ViewValues.FAST_TIME_ANIMATION);
            _message.text = "<size=100%>" + _value + (string.IsNullOrEmpty(_description) ? string.Empty : "\n <size=60%>" + _description);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(1f, ViewValues.FAST_TIME_ANIMATION);
            _message.text = string.Empty;
        }
    }
}

