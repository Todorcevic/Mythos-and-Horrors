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
        [SerializeField, Required, ChildGameObjectsOnly] private Image _frame;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _value;
        private string _description;
        private ChallengeMessageController _challengeMessage;

        public ChallengeToken Challengetoken { get; private set; }

        /*******************************************************************/
        public void SetImage(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        public void SetToken(ChallengeToken challengetoken, int value, string description, ChallengeMessageController challengeMessage)
        {
            Challengetoken = challengetoken;
            _challengeMessage = challengeMessage;
            _description = description;
            _value.text = value.ToString();
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
            _frame.gameObject.SetActive(true);
            _value.transform.parent.gameObject.SetActive(true);
            ShowText();
        }

        public void HideToken()
        {
            _frame.gameObject.SetActive(false);
            _value.transform.parent.gameObject.SetActive(false);
            HideText();
        }

        private void ShowText()
        {
            _challengeMessage.ShowThisToken(_value.text, _description, _image.sprite);
        }

        private void HideText()
        {
            _challengeMessage.ShowLastDropTokens();
        }
    }
}

