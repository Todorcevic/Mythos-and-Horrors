using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace MythosAndHorrors.GameView
{
    public class SkillChallengeController : MonoBehaviour
    {
        [SerializeField, Required, AssetsOnly] private Sprite _strength;
        [SerializeField, Required, AssetsOnly] private Sprite _agility;
        [SerializeField, Required, AssetsOnly] private Sprite _intelligence;
        [SerializeField, Required, AssetsOnly] private Sprite _power;

        [SerializeField, Required, ChildGameObjectsOnly] private Image _skill;

        /*******************************************************************/
        public Tween SetSkill(ChallengeType challengeType)
        {
            _skill.sprite = challengeType switch
            {
                ChallengeType.Strength => _strength,
                ChallengeType.Agility => _agility,
                ChallengeType.Intelligence => _intelligence,
                ChallengeType.Power => _power,
                _ => null
            };

            return transform.DOScale(1, 0).SetEase(Ease.OutBack);
        }

        public Tween ShutDown() => transform.DOScale(0, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.InBack);
    }
}

