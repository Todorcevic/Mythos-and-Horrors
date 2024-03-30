using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    [CreateAssetMenu(fileName = "Faction", menuName = "ScriptableObjects/Tokens")]
    public class ChallengeTokenSO : ScriptableObject
    {
        [SerializeField, Required] private Sprite _image;
        [SerializeField, Required] private ChallengeTokenType _type;

        public Sprite Image => _image;
        public ChallengeTokenType TokenType => _type;
    }
}

