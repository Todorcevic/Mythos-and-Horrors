using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class MainButtonController : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private MeshRenderer _buttonRenderer;
        [SerializeField, Required, ChildGameObjectsOnly] private Light _light;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _message;
        [SerializeField, Required, ChildGameObjectsOnly] private BoxCollider _collider;
        [SerializeField, Required] private Color _activateColor;
        [SerializeField, Required] private Color _deactivateColor;
        [Inject] private readonly InteractableHandler _interactablePresenter;
        [Inject] private readonly MultiEffectSelectionHandler _multiEffectSelectionHandler;

        private bool IsActivated => _collider.enabled;

        /*******************************************************************/
        public void ShowText(string text)
        {
            _message.text = text;
        }

        public void Activate()
        {
            if (IsActivated) return;
            _collider.enabled = true;
            _buttonRenderer.transform.DOScaleZ(1f, ViewValues.FAST_TIME_ANIMATION * 0.5f).SetEase(Ease.Linear);
            _buttonRenderer.material.DOColor(_activateColor, ViewValues.FAST_TIME_ANIMATION);

            _message.transform.DOScale(Vector3.one * 0.005f, ViewValues.FAST_TIME_ANIMATION).SetEase(Ease.InOutBack, 3f);
            _message.DOFade(1f, ViewValues.FAST_TIME_ANIMATION);
        }

        public void Deactivate()
        {
            if (!IsActivated) return;
            _collider.enabled = false;
            _buttonRenderer.transform.DOScaleZ(0.75f, ViewValues.FAST_TIME_ANIMATION * 0.5f).SetEase(Ease.Linear);
            _buttonRenderer.material.DOColor(_deactivateColor, ViewValues.FAST_TIME_ANIMATION);
            _message.transform.DOScale(Vector3.zero, ViewValues.FAST_TIME_ANIMATION);
            _message.DOFade(0f, ViewValues.FAST_TIME_ANIMATION);
            _light.DOIntensity(0f, ViewValues.FAST_TIME_ANIMATION);
        }

        public void OnMouseEnter()
        {
            _light.DOIntensity(2f, ViewValues.FAST_TIME_ANIMATION);
        }

        public void OnMouseExit()
        {
            _light.DOIntensity(0f, ViewValues.FAST_TIME_ANIMATION);
        }

        public void OnMouseUpAsButton()
        {
            _interactablePresenter.Clicked();
            _multiEffectSelectionHandler.Clicked();
        }
    }
}
