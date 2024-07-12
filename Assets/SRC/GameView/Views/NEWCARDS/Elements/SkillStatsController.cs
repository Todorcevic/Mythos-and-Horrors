using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView.NEWS
{

    public class SkillStatsController : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _strength;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _agility;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _intelligence;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _power;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _damage;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _fear;

        /*******************************************************************/
        public void SetStats(Card Card)
        {
            if (Card is CardInvestigator _cardInvestigator)
            {
                _strength.SetStat(_cardInvestigator.Strength);
                _agility.SetStat(_cardInvestigator.Agility);
                _intelligence.SetStat(_cardInvestigator.Intelligence);
                _power.SetStat(_cardInvestigator.Power);
            }
        }
    }
}
