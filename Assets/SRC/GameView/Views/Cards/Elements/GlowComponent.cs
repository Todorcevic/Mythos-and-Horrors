using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class GlowComponent : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _spriteRenderer;

        /*******************************************************************/
        public Tween Off()
        {
            return _spriteRenderer.DOFade(0f, ViewValues.FAST_TIME_ANIMATION);
        }

        public Tween SetGreenGlow()
        {
            return _spriteRenderer.DOFade(1f, ViewValues.FAST_TIME_ANIMATION);
        }
    }
}
