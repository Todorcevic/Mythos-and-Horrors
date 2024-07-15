using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class SkillStatsController : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _strength;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _agility;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _intelligence;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _power;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _wild;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _damage;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _fear;

        /*******************************************************************/
        public void Init(Card card)
        {
            switch (card)
            {
                case CardInvestigator cardInvestigator:
                    SetStrenghtWith(cardInvestigator.Strength);
                    SetAgilityWith(cardInvestigator.Agility);
                    SetIntelligenceWith(cardInvestigator.Intelligence);
                    SetPowerWith(cardInvestigator.Power);
                    break;

                case CardCreature cardCreature:
                    SetStrenghtWith(cardCreature.Strength);
                    SetAgilityWith(cardCreature.Agility);
                    SetDamageWith(cardCreature.Damage);
                    SetFearWith(cardCreature.Fear);
                    break;

                case CommitableCard commitableCard:
                    if (commitableCard.Strength.Value > 0) SetStrenghtWith(commitableCard.Strength);
                    if (commitableCard.Agility.Value > 0) SetAgilityWith(commitableCard.Agility);
                    if (commitableCard.Intelligence.Value > 0) SetIntelligenceWith(commitableCard.Intelligence);
                    if (commitableCard.Power.Value > 0) SetPowerWith(commitableCard.Power);
                    if (commitableCard.Wild.Value > 0) SetWildWith(commitableCard.Wild);
                    break;

                case CardPlace cardPlace:
                    SetIntelligenceWith(cardPlace.Enigma);
                    break;

                    //TODO: ChallengeCards with typeChallenge 
            }
        }

        private void SetStrenghtWith(Stat stat)
        {
            _strength.SetStat(stat);
            _strength.gameObject.SetActive(true);
        }

        private void SetAgilityWith(Stat stat)
        {
            _agility.SetStat(stat);
            _agility.gameObject.SetActive(true);
        }

        private void SetIntelligenceWith(Stat stat)
        {
            _intelligence.SetStat(stat);
            _intelligence.gameObject.SetActive(true);
        }

        private void SetPowerWith(Stat stat)
        {
            _power.SetStat(stat);
            _power.gameObject.SetActive(true);
        }

        private void SetWildWith(Stat stat)
        {
            _wild.SetStat(stat);
            _wild.gameObject.SetActive(true);
        }

        private void SetDamageWith(Stat stat)
        {
            _damage.SetStat(stat);
            _damage.gameObject.SetActive(true);
        }

        private void SetFearWith(Stat stat)
        {
            _fear.SetStat(stat);
            _fear.gameObject.SetActive(true);
        }
    }
}
