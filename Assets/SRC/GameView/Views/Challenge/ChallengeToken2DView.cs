using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MythosAndHorrors.GameView
{
    public class ChallengeToken2DView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField, Required, ChildGameObjectsOnly] private Image _image;
        private string _value;
        private string _description;
        private TextMeshProUGUI _message;

        public ChallengeToken Challengetoken { get; private set; }

        /*******************************************************************/
        public void SetImage(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        public void SetToken(ChallengeToken challengetoken, int value, string description, TextMeshProUGUI message)
        {
            Challengetoken = challengetoken;
            _message = message;
            _description = description;
            _value = $"{(value > 0 ? "+" : "")}{value}";
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            _image.DOColor(new Color(0.8f, 0.8f, 0.8f), ViewValues.FAST_TIME_ANIMATION).SetNotWaitable();
            ShowText();
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            _image.DOColor(Color.white, ViewValues.FAST_TIME_ANIMATION).SetNotWaitable();
            HideText();
        }

        public void ShowToken()
        {
            transform.DOScale(1.2f, ViewValues.FAST_TIME_ANIMATION);
            ShowText();
        }

        public void HideToken()
        {
            transform.DOScale(1f, ViewValues.FAST_TIME_ANIMATION);
            HideText();
        }

        private void ShowText() => _message.text = "<size=100%>" + _value + (string.IsNullOrEmpty(_description) ? string.Empty : "\n <size=60%>" + _description);

        private void HideText() => _message.text = string.Empty;
    }
}

