using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class CloneComponent : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private EffectController _effectController;
        [SerializeField, Required, ChildGameObjectsOnly] private EffectController _buffsController;
        [SerializeField, Required, ChildGameObjectsOnly] private GlowController _glowComponent;
        [Inject] private readonly DiContainer _diContainer;

        /*******************************************************************/
        public CloneComponent Clone(Transform parent)
        {
            CloneComponent clone = _diContainer.InstantiatePrefabForComponent<CloneComponent>(gameObject, parent);
            clone.transform.ResetToZero();
            clone._effectController.gameObject.SetActive(true);
            clone._buffsController.gameObject.SetActive(true);
            return clone;
        }

        public Tween Animation() => DOTween.Sequence()
                .Join(_glowComponent.Off())
                .Join(transform.DORecolocate().SetEase(Ease.InOutExpo));
    }
}
