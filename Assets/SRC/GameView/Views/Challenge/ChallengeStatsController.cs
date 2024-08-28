using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class ChallengeStatsController : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _strengthValue;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _agilityValue;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _intelligenceValue;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _powerValue;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _wildValue;

        public void SetStat(ChallengeType challengeType, int value)
        {
            HideAll();
            if (challengeType == ChallengeType.Strength)
            {
                _strengthValue.text = value.ToString();
                _strengthValue.transform.parent.gameObject.SetActive(true);
            }
            else if (challengeType == ChallengeType.Agility)
            {
                _agilityValue.text = value.ToString();
                _agilityValue.transform.parent.gameObject.SetActive(true);
            }
            else if (challengeType == ChallengeType.Intelligence)
            {
                _intelligenceValue.text = value.ToString();
                _intelligenceValue.transform.parent.gameObject.SetActive(true);
            }
            else if (challengeType == ChallengeType.Power)
            {
                _powerValue.text = value.ToString();
                _powerValue.transform.parent.gameObject.SetActive(true);
            }
        }

        public void SetWildStat(int value)
        {
            _wildValue.text = value.ToString();
            _wildValue.gameObject.SetActive(true);
        }

        private void HideAll()
        {
            _strengthValue.transform.parent.gameObject.SetActive(false);
            _agilityValue.transform.parent.gameObject.SetActive(false);
            _intelligenceValue.transform.parent.gameObject.SetActive(false);
            _powerValue.transform.parent.gameObject.SetActive(false);
            _wildValue.transform.parent.gameObject.SetActive(false);
        }
    }
}
