using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MythsAndHorrors.GameView
{
    public class AvatarView : MonoBehaviour
    {
        private Adventurer _adventurer;
        [SerializeField, Required, ChildGameObjectsOnly] private Image _picture;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _health;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _sanity;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _hints;

        /*******************************************************************/
        public void Init(Adventurer adventurer)
        {
            _adventurer = adventurer;
            //_picture.sprite = adventurer.AdventurerCard.Info.Picture;
            _health.text = adventurer.AdventurerCard.Info.Health.ToString();
            _sanity.text = adventurer.AdventurerCard.Info.Sanity.ToString();
            _hints.text = adventurer.AdventurerCard.Info.Hints.ToString();
        }
    }
}
