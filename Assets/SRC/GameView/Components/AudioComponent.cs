using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class AudioComponent : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private AudioSource _audioSource;

        /*******************************************************************/
        public async Task PlayAudio(AudioClip audioClip)
        {
            if (audioClip == null) return;
            _audioSource.PlayOneShot(audioClip);
            int duration = Mathf.CeilToInt(audioClip.length * 1000);
            await Task.Delay(duration);
        }
    }
}