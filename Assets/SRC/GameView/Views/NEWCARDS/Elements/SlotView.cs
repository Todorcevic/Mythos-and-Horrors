using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView.NEWS
{
    public class SlotView : MonoBehaviour
    {
        [SerializeField, Required, AssetsOnly] private Sprite _supporter;
        [SerializeField, Required, AssetsOnly] private Sprite _item;
        [SerializeField, Required, AssetsOnly] private Sprite _magical;
        [SerializeField, Required, AssetsOnly] private Sprite _trinket;
        [SerializeField, Required, AssetsOnly] private Sprite _equipment;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _icon;

        /*******************************************************************/
        public void Init(SlotType slotType)
        {
            switch (slotType)
            {
                case SlotType.Supporter:
                    SetSlotWith(_supporter);
                    break;
                case SlotType.Item:
                    SetSlotWith(_item);
                    break;
                case SlotType.Magical:
                    SetSlotWith(_magical);
                    break;
                case SlotType.Trinket:
                    SetSlotWith(_trinket);
                    break;
                case SlotType.Equipment:
                    SetSlotWith(_equipment);
                    break;
                default:
                    gameObject.SetActive(false);
                    break;
            }
        }

        private void SetSlotWith(Sprite icon)
        {
            _icon.sprite = icon;
        }
    }
}
