using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class SceneTokenView : MonoBehaviour
    {
        ChallengeToken _challengeToken;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _value;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _description;

        /*******************************************************************/
        public void SetChallengeToken(ChallengeToken challengeToken)
        {
            _challengeToken = challengeToken;
            _description.text = _challengeToken.Description;
        }

        public void UpdateValue()
        {
            _value.text = _challengeToken.Value.Invoke().ToString();
        }
    }
}

