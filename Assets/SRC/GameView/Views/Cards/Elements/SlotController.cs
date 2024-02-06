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
        public void SetSlot(Slot slot)
        {
            switch (slot)
            {
                case Slot.Trinket:
                    _slot1.sprite = _trinket;
                    _slot2.gameObject.SetActive(false);
                    break;
                case Slot.Equipment:
                    _slot1.sprite = _equipment;
                    _slot2.gameObject.SetActive(false);
                    break;
                case Slot.Supporter:
                    _slot1.sprite = _supporter;
                    _slot2.gameObject.SetActive(false);
                    break;
                case Slot.Item:
                    _slot1.sprite = _item;
                    _slot2.gameObject.SetActive(false);
                    break;
                case Slot.Itemx2:
                    _slot1.sprite = _item;
                    _slot2.sprite = _item;
                    break;
                case Slot.Magical:
                    _slot1.sprite = _magical;
                    _slot2.gameObject.SetActive(false);
                    break;
                case Slot.Magicalx2:
                    _slot1.sprite = _magical;
                    _slot2.sprite = _magical;
                    break;
                default:
                    _slot1.gameObject.SetActive(false);
                    _slot2.gameObject.SetActive(false);
                    break;
            }
            DoPermisive();
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
