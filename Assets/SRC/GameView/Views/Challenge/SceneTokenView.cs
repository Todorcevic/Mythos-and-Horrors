using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MythosAndHorrors.GameView
{
    public class SceneTokenView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _value;
        [SerializeField, Required, ChildGameObjectsOnly] private Image _image;
        private string _description;
        private TextMeshProUGUI _message;

        /*******************************************************************/
        public void SetToken(int value, string description, Sprite image, TextMeshProUGUI message)
        {
            _message = message;
            _description = description;
            _image.sprite = image;
            _value.text = $"{(value > 0 ? "+" : "")}{value}";
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(1.2f, ViewValues.FAST_TIME_ANIMATION);
            _message.text = "<size=100%>" + _value.text + (string.IsNullOrEmpty(_description) ? string.Empty : "\n <size=60%>" + _description);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(1f, ViewValues.FAST_TIME_ANIMATION);
            _message.text = string.Empty;
        }
    }
}

