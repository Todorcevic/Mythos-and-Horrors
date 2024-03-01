using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class MainButtonComponent : MonoBehaviour, IPlayable
    {
        private const float OFFSET = 1f;
        List<Effect> _effects = new();
        [Inject] private readonly ClickHandler<IPlayable> _clickHandler;
        [SerializeField, Required, ChildGameObjectsOnly] private MeshRenderer _buttonRenderer;
        [SerializeField, Required, ChildGameObjectsOnly] private Light _light;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _message;
        [SerializeField, Required, ChildGameObjectsOnly] private BoxCollider _collider;
        [SerializeField, Required] private Color _activateColor;
        [SerializeField, Required] private Color _deactivateColor;

        private bool IsActivated => _collider.enabled;

        List<Effect> IPlayable.EffectsSelected => _effects;

        /*******************************************************************/
        public void SetButton(string text, List<Effect> effects)
        {
            _message.text = text;
            _effects = effects;
        }

        public void Clear()
        {
            _message.text = string.Empty;
            _effects.Clear();
        }

        public void ActivateToClick()
        {
            if (IsActivated) return;
            _collider.enabled = true;
            _buttonRenderer.transform.DOScaleZ(1f, ViewValues.FAST_TIME_ANIMATION * 0.5f).SetEase(Ease.Linear);
            _buttonRenderer.material.DOColor(_activateColor, ViewValues.FAST_TIME_ANIMATION);
            _message.transform.DOScale(Vector3.one * 0.005f, ViewValues.FAST_TIME_ANIMATION).SetEase(Ease.InOutBack, 3f);
            _message.DOFade(1f, ViewValues.FAST_TIME_ANIMATION);
        }

        public void DeactivateToClick()
        {
            if (!IsActivated) return;
            _collider.enabled = false;
            _buttonRenderer.transform.DOScaleZ(0.75f, ViewValues.FAST_TIME_ANIMATION * 0.5f).SetEase(Ease.Linear);
            _buttonRenderer.material.DOColor(_deactivateColor, ViewValues.FAST_TIME_ANIMATION);
            _message.transform.DOScale(Vector3.zero, ViewValues.FAST_TIME_ANIMATION);
            _message.DOFade(0f, ViewValues.FAST_TIME_ANIMATION);
            _light.DOIntensity(0f, ViewValues.FAST_TIME_ANIMATION);
        }

        public Tween MoveToShowSelector(Transform scenePoint)
        {
            if (!((IPlayable)this).CanBePlayed) return DOTween.Sequence();
            return DOTween.Sequence()
                     .Join(transform.DOMove(ButtonPositionInUI(), ViewValues.DEFAULT_TIME_ANIMATION))
                     .Join(transform.DOScale(scenePoint.lossyScale, ViewValues.DEFAULT_TIME_ANIMATION))
                     .SetEase(Ease.InOutCubic);

            Vector3 ButtonPositionInUI()
            {
                float distanceBorderRight = (OFFSET - (Screen.width - Camera.main.WorldToScreenPoint(scenePoint.position).x) / Screen.width);
                return new Vector3(scenePoint.position.x - distanceBorderRight, scenePoint.position.y, scenePoint.position.z);
            }
        }

        public Tween RestorePosition() => DOTween.Sequence()
            .Join(transform.DOLocalMove(Vector3.zero, ViewValues.DEFAULT_TIME_ANIMATION))
            .Join(transform.DOScale(Vector3.one, ViewValues.DEFAULT_TIME_ANIMATION))
            .Join(transform.DOLocalRotate(Vector3.zero, ViewValues.DEFAULT_TIME_ANIMATION))
            .SetEase(Ease.InSine);

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
            _clickHandler.Clicked(this);
        }

        //void IPlayable.ActivateToClick()
        //{
        //    throw new System.NotImplementedException();
        //}

        //void IPlayable.DeactivateToClick()
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}
