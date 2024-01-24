using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class MainButtonComponent : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private MeshRenderer _buttonRenderer;
        [SerializeField, Required, ChildGameObjectsOnly] private Light _light;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _message;
        [SerializeField, Required, ChildGameObjectsOnly] private BoxCollider _collider;
        [SerializeField, Required] private Color _activateColor;
        [SerializeField, Required] private Color _deactivateColor;
        [Inject] private readonly IInteractable _interactable;
        private const float OFFSET = 120f;
        private const float SCALE = 0.05f;

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

        public Tween MoveToThis(Transform scenePoint)
        {
            return DOTween.Sequence().OnStart(() => transform.ChangeAllLayers(3))
                     .Join(transform.DOMove(ButtonPositionInUI(), ViewValues.DEFAULT_TIME_ANIMATION))
                     .Join(transform.DOScale(scenePoint.localScale, ViewValues.DEFAULT_TIME_ANIMATION));

            Vector3 ButtonPositionInUI()
            {
                float distanceBorderRight = (OFFSET - (Screen.width - Camera.main.WorldToScreenPoint(scenePoint.position).x)) * SCALE;
                return new Vector3(scenePoint.position.x - distanceBorderRight, scenePoint.position.y, scenePoint.position.z);
            }
        }

        public Tween RestorePosition() => DOTween.Sequence()
            .Join(transform.DOLocalMove(Vector3.zero, ViewValues.DEFAULT_TIME_ANIMATION))
            .Join(transform.DOLocalRotate(Vector3.zero, ViewValues.DEFAULT_TIME_ANIMATION));

        /*******************************************************************/
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
            _interactable.Clicked(null);
        }
    }
}
