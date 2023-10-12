using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class CardHolderView : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _title;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _description;
        [SerializeField, ChildGameObjectsOnly] private TextMeshPro _health;
        [SerializeField, ChildGameObjectsOnly] private TextMeshPro _sanity;
        [SerializeField, ChildGameObjectsOnly] private TextMeshPro _strength;
        [SerializeField, ChildGameObjectsOnly] private TextMeshPro _agility;
        [SerializeField, ChildGameObjectsOnly] private TextMeshPro _intelligence;
        [SerializeField, ChildGameObjectsOnly] private TextMeshPro _power;
        [SerializeField, ChildGameObjectsOnly] private TextMeshPro _cost;
        [SerializeField, ChildGameObjectsOnly] private TextMeshPro _hints;
        [SerializeField, ChildGameObjectsOnly] private TextMeshPro _enigma;
        [SerializeField, ChildGameObjectsOnly] private TextMeshPro _eldritch;

        /*******************************************************************/
        public void SetInfo(Card thisCard)
        {
            if (_title != null) _title.text = thisCard.Info.Name;
            if (_description != null) _description.text = thisCard.Info.Description;
            if (_health != null) _health.text = thisCard.Info.Health.ToString();
            if (_sanity != null) _sanity.text = thisCard.Info.Sanity.ToString();
            if (_strength != null) _strength.text = thisCard.Info.Strength.ToString();
            if (_agility != null) _agility.text = thisCard.Info.Agility.ToString();
            if (_intelligence != null) _intelligence.text = thisCard.Info.Intelligence.ToString();
            if (_power != null) _power.text = thisCard.Info.Power.ToString();
            if (_cost != null) _cost.text = thisCard.Info.Cost.ToString();
            if (_hints != null) _hints.text = thisCard.Info.Hints.ToString();
            if (_enigma != null) _enigma.text = thisCard.Info.Enigma.ToString();
            if (_eldritch != null) _eldritch.text = thisCard.Info.Eldritch.ToString();
        }
    }
}
