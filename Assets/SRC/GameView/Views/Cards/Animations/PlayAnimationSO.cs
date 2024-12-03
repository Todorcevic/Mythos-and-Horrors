using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    [CreateAssetMenu(fileName = "PlayAnimation", menuName = "ScriptableObjects/PlayAnimation")]
    public class PlayAnimationSO : ScriptableObject
    {
        [SerializeField, Required] private List<AudioClip> _audios;
        public List<AudioClip> Audio => _audios;

        public AudioClip GetAudioByCode(string name) => _audios.Find(audio => audio.name == name);
    }
}

