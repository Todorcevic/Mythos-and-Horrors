using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class GlowController : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _spriteRenderer;
        [SerializeField, Required, AssetsOnly] private Material _active;
        [SerializeField, Required, AssetsOnly] private Material _off;

        /*******************************************************************/
        public Tween Off()
        {
            _spriteRenderer.material = _off;
            return Animation();
        }

        public Tween SetGreenGlow()
        {
            _spriteRenderer.material = _active;
            return Animation();
        }

        private Tween Animation()
        {
            Vector3 currentScale = _spriteRenderer.transform.localScale;
            _spriteRenderer.transform.localScale = currentScale * 0.8f;
            return _spriteRenderer.transform.DOScale(currentScale, ViewValues.FAST_TIME_ANIMATION);
        }
    }
}
