using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    [CreateAssetMenu(fileName = "PlayAnimation", menuName = "ScriptableObjects/PlayAnimation")]
    public class PlayAnimationSO : ScriptableObject
    {
        [SerializeField, Required] private string _localizableCode;
        [SerializeField, Required] private AudioClip _audio;

        public string LocalizableCode => _localizableCode;
        public AudioClip Audio => _audio;
    }
}

