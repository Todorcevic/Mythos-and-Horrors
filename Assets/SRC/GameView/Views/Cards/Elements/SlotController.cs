using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class SlotController : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _slot1;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _slot2;
        [SerializeField, Required, AssetsOnly] private Sprite _trinket;
        [SerializeField, Required, AssetsOnly] private Sprite _equipment;
        [SerializeField, Required, AssetsOnly] private Sprite _supporter;
        [SerializeField, Required, AssetsOnly] private Sprite _item;
        [SerializeField, Required, AssetsOnly] private Sprite _magical;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _permisiveColor;
        [SerializeField] private Color _restrictedColor;

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
                        ActivateSlot(_trinket);
                        break;
                    case SlotType.Supporter:
                        ActivateSlot(_trinket);
                        break;
                    case SlotType.Item:
                        ActivateSlot(_trinket);
                        break;
                    case SlotType.Magical:
                        ActivateSlot(_trinket);
                        break;
                    default:
                        _slot1.transform.parent.gameObject.SetActive(false);
                        _slot2.transform.parent.gameObject.SetActive(false);
                        break;
                }
            }
            DoPermisive();
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
            _slot1.material.color = _defaultColor;
            _slot2.material.color = _defaultColor;
        }

        public void DoPermisive()
        {
            _slot1.material.color = _permisiveColor;
            _slot2.material.color = _permisiveColor;
        }

        public void DoRestricted()
        {
            _slot1.material.color = _restrictedColor;
            _slot2.material.color = _restrictedColor;
        }
    }
}
