using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class AudioComponent : MonoBehaviour
    {
        [SerializeField, Required, AssetList] private List<PlayAnimationSO> allAnimations;
        [SerializeField, Required, ChildGameObjectsOnly] private AudioSource _audioSource;

        /*******************************************************************/
        public void PlayAudio(string localizableCode)
        {
            PlayAnimationSO soundToPlay = allAnimations.Find(playAnimation => playAnimation.LocalizableCode == localizableCode);
            _audioSource.PlayOneShot(soundToPlay.Audio);
        }
    }
}