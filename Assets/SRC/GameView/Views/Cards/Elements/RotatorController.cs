using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class RotatorController : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _rotator;
        [SerializeField, Required, AssetsOnly] private AudioClip _drawAudio;
        [SerializeField, Required, AssetsOnly] private AudioClip _360Audio;
        [Inject] private readonly AudioComponent _audioComponent;
        private bool _isRotate;

        /*******************************************************************/
        public Tween Rotate(bool rotate, bool isInDeck)
        {
            if (_isRotate == rotate) return DOTween.Sequence();
            _isRotate = rotate;
            return _rotator.DOLocalRotate(new Vector3(0, 0, rotate ? 180 : 0), ViewValues.FAST_TIME_ANIMATION)
            .OnPlay(() => _audioComponent.PlayAudio(isInDeck ? _drawAudio : null))
            .SetDelay(0.1f).SetEase(Ease.InOutSine);
        }

        public Tween Rotate360(float timeAnimation = ViewValues.SLOW_TIME_ANIMATION) =>
            _rotator.DOLocalRotate(new Vector3(0, 0, 360), timeAnimation, mode: RotateMode.FastBeyond360)
            .OnComplete(() => _rotator.localEulerAngles = Vector3.zero);

        public Tween RotateFake(float timeAnimation = ViewValues.DEFAULT_TIME_ANIMATION) => DOTween.Sequence()
            .OnPlay(() => _audioComponent.PlayAudio(_360Audio))
            .Append(_rotator.DOLocalRotate(new Vector3(0, 0, 90), timeAnimation * 0.5f).SetEase(Ease.Linear))
            .Append(_rotator.DOLocalRotate(new Vector3(0, 0, 270), 0f))
            .Append(_rotator.DOLocalRotate(Vector3.zero, timeAnimation * 0.5f).SetEase(Ease.Linear));
    }
}
