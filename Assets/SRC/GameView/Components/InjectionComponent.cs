using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class InjectionComponent : MonoInstaller
    {
        [SerializeField, Required, AssetsOnly] private List<PlayAnimationSO> _allAnimations;

        /*******************************************************************/
        public override void InstallBindings()
        {
            Container.Install<InjectionService>();
            Container.BindInstance(_allAnimations).AsSingle();
        }
    }
}