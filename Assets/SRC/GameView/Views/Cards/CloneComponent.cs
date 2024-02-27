using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class CloneComponent : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private EffectController _effectController;
        [SerializeField, Required, ChildGameObjectsOnly] private GlowController _glowComponent;
        [Inject] private readonly DiContainer _diContainer;

        /*******************************************************************/
        public CloneComponent Clone(Transform parent) => _diContainer.InstantiatePrefabForComponent<CloneComponent>(gameObject, parent);

        public void ShowEffects()
        {
            _effectController.gameObject.SetActive(true);
        }

        public Tween DisableGlow() => _glowComponent.Off();
    }
}
