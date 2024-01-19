using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ButtonController : MonoBehaviour
    {
        private bool _isActivated = true;

        [SerializeField, Required, ChildGameObjectsOnly] private MeshRenderer _buttonRenderer;
        [SerializeField, Required, ChildGameObjectsOnly] private Light _light;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _message;
        [SerializeField, Required] private Color _activateColor;
        [SerializeField, Required] private Color _deactivateColor;
        [Inject] private readonly InteractablePresenter _interactablePresenter;

        /*******************************************************************/
        public void ShowText(string text)
        {
            _message.text = text;
        }

        public void Activate()
        {
            if (_isActivated) return;
            _isActivated = true;
            _buttonRenderer.transform.DOScaleZ(1f, ViewValues.FAST_TIME_ANIMATION * 0.5f).SetEase(Ease.Linear);
            _buttonRenderer.material.DOColor(_activateColor, ViewValues.FAST_TIME_ANIMATION);

            _message.transform.DOScale(Vector3.one * 0.005f, ViewValues.FAST_TIME_ANIMATION).SetEase(Ease.InOutBack, 3f);
            _message.DOFade(1f, ViewValues.FAST_TIME_ANIMATION);
        }

        public void Deactivate()
        {
            if (!_isActivated) return;
            _isActivated = false;
            _buttonRenderer.transform.DOScaleZ(0.75f, ViewValues.FAST_TIME_ANIMATION * 0.5f).SetEase(Ease.Linear);
            _buttonRenderer.material.DOColor(_deactivateColor, ViewValues.FAST_TIME_ANIMATION);
            _message.transform.DOScale(Vector3.zero, ViewValues.FAST_TIME_ANIMATION);
            _message.DOFade(0f, ViewValues.FAST_TIME_ANIMATION);
            _light.DOIntensity(0f, ViewValues.FAST_TIME_ANIMATION);
        }

        public void OnMouseEnter()
        {
            if (!_isActivated) return;
            _light.DOIntensity(2f, ViewValues.FAST_TIME_ANIMATION);
        }

        public void OnMouseExit()
        {
            _light.DOIntensity(0f, ViewValues.FAST_TIME_ANIMATION);
        }

        public void OnMouseUpAsButton()
        {
            if (!_isActivated) return;
            _interactablePresenter.Clicked();
        }
    }
}
