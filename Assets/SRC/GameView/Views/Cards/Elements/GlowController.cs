using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class GlowController : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _spriteRenderer;
        [SerializeField, Required, AssetsOnly] private Material _active;
        [SerializeField, Required, AssetsOnly] private Material _off;
        [SerializeField, Required, AssetsOnly] private AudioClip _glowOnAudio;
        [SerializeField, Required, AssetsOnly] private AudioClip _glowOffAudio;
        [Inject] private readonly AudioComponent _audioComponent;

        /*******************************************************************/
        public Tween SetGreenGlow()
        {
            _spriteRenderer.material = _active;
            return Animation().OnPlay(() => _audioComponent.PlayAudio(_glowOnAudio));
        }

        public Tween Off()
        {
            _spriteRenderer.material = _off;
            return Animation().OnPlay(() => _audioComponent.PlayAudio(_glowOffAudio));
        }

        private Tween Animation()
        {
            Vector3 currentScale = _spriteRenderer.transform.localScale;
            _spriteRenderer.transform.localScale = currentScale * 0.8f;
            return _spriteRenderer.transform.DOScale(currentScale, ViewValues.FAST_TIME_ANIMATION);
        }
    }
}
