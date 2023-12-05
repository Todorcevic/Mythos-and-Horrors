using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MythsAndHorrors.GameView
{
    public class AvatarView : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private Image _picture;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _health;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _sanity;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _hints;
        [SerializeField, Required, ChildGameObjectsOnly] private TurnController _turnController;

        public bool IsVoid => Adventurer == null;
        public Adventurer Adventurer { get; private set; }

        /*******************************************************************/
        public void Init(Adventurer adventurer)
        {
            Adventurer = adventurer;
            //_picture.sprite = adventurer.AdventurerCard.Info.Picture;
            SetHealth(adventurer.AdventurerCard.Info.Health ?? 0);
            SetSanity(adventurer.AdventurerCard.Info.Sanity ?? 0);
            SetHints(adventurer.AdventurerCard.Info.Hints ?? 0);
            gameObject.SetActive(true);
        }

        public void SetHealth(int amount) => _health.text = amount.ToString();

        public void SetSanity(int amount) => _sanity.text = amount.ToString();

        public void SetHints(int amount) => _hints.text = amount.ToString();
    }
}
