using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
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

        public void ShowEffects(Effect[] effects) => _effectController.AddEffects(effects);

        public void DisableGlow() => _glowComponent.gameObject.SetActive(false);
    }
}
