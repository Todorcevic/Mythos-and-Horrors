using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    [CreateAssetMenu(fileName = "ChallengeToken", menuName = "ScriptableObjects/Tokens")]
    public class ChallengeTokenSO : ScriptableObject
    {
        [SerializeField, Required] private Sprite _image;
        [SerializeField, Required] private Texture _texture;
        [SerializeField, Required] private AudioClip _audioClip;
        [SerializeField, Required] private ChallengeTokenType _type;

        public Sprite Image => _image;
        public Texture Texture => _texture;
        public AudioClip AudioClip => _audioClip;
        public ChallengeTokenType TokenType => _type;
    }
}

