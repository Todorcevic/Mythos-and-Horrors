using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace MythsAndHorrors.GameView
{
    public class SelectorBlockController : MonoBehaviour
    {
        [SerializeField, Required, SceneObjectsOnly] private BoxCollider _selectorBlock;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _selectorBackground;
        [SerializeField, Required, SceneObjectsOnly] private Image _avatarBlock;

        public bool IsActivated => _selectorBlock.enabled;

        /*******************************************************************/
        public Tween ActivateSelector()
        {
            return _selectorBackground.DOFade(ViewValues.DEFAULT_FADE, ViewValues.DEFAULT_TIME_ANIMATION)
                .SetEase(Ease.Linear)
                .OnStart(() => _avatarBlock.raycastTarget = _selectorBlock.enabled = true);
        }

        public Tween DeactivateSelector()
        {
            return _selectorBackground.DOFade(0f, ViewValues.DEFAULT_TIME_ANIMATION)
                .SetEase(Ease.Linear)
                .OnComplete(() => _avatarBlock.raycastTarget = _selectorBlock.enabled = false);
        }
    }
}
