using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class SlotController : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _slot1;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _slot2;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _holder1;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _holder2;
        [SerializeField, Required, AssetsOnly] private Sprite _trinket;
        [SerializeField, Required, AssetsOnly] private Sprite _equipment;
        [SerializeField, Required, AssetsOnly] private Sprite _supporter;
        [SerializeField, Required, AssetsOnly] private Sprite _item;
        [SerializeField, Required, AssetsOnly] private Sprite _magical;

        /*******************************************************************/
        public void SetSlots(SlotType[] slots)
        {
            foreach (SlotType slot in slots)
            {
                switch (slot)
                {
                    case SlotType.Trinket:
                        ActivateSlot(_trinket);
                        break;
                    case SlotType.Equipment:
                        ActivateSlot(_equipment);
                        break;
                    case SlotType.Supporter:
                        ActivateSlot(_supporter);
                        break;
                    case SlotType.Item:
                        ActivateSlot(_item);
                        break;
                    case SlotType.Magical:
                        ActivateSlot(_magical);
                        break;
                    default:
                        _slot1.transform.parent.gameObject.SetActive(false);
                        _slot2.transform.parent.gameObject.SetActive(false);
                        break;
                }
            }
        }

        private void ActivateSlot(Sprite withThis)
        {
            if (_slot1.sprite == null)
            {
                _slot1.sprite = withThis;
                _slot1.transform.parent.gameObject.SetActive(true);
            }
            else
            {
                _slot2.sprite = withThis;
                _slot2.transform.parent.gameObject.SetActive(true);
            }
        }

        public void DoDefault()
        {
            _slot1.material.DisableKeyword("_EMISSION");
            _slot2.material.DisableKeyword("_EMISSION");
        }

        public void DoActive(int amount)
        {
            _slot1.material.EnableKeyword("_EMISSION");
            _slot2.material.EnableKeyword("_EMISSION");
            _slot1.material.SetColor("_EmissionColor", ViewValues.DEACTIVE_COLOR);
            _slot2.material.SetColor("_EmissionColor", ViewValues.DEACTIVE_COLOR);

            if (amount < 1) return;
            _slot1.material.SetColor("_EmissionColor", ViewValues.ACTIVE_COLOR);

            if (amount < 2) return;
            _slot2.material.SetColor("_EmissionColor", ViewValues.ACTIVE_COLOR);
        }
    }
}
