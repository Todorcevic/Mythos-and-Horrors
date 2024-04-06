using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class SceneTokenView : MonoBehaviour
    {
        private ChallengeToken _challengeToken;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _value;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _description;

        /*******************************************************************/
        public void SetToken(ChallengeToken challengeToken)
        {
            if (challengeToken == null) return;
            gameObject.SetActive(true);
            _challengeToken = challengeToken;
            _description.text = _challengeToken.Description;
        }

        public void UpdateValue()
        {
            if (_challengeToken == null) return;
            _value.text = (_challengeToken.Value?.Invoke() ?? 0).ToString();
        }
    }
}

