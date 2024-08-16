using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class AudioComponent : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private AudioSource _audioSource;

        /*******************************************************************/
        public void PlayAudio(AudioClip audio)
        {
            _audioSource.PlayOneShot(audio);
        }
    }
}