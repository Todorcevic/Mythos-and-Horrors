using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Net;
using TMPro;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class CostController : MonoBehaviour, IStatable
    {
        [SerializeField, Required, ChildGameObjectsOnly] private SlotView _slot1;
        [SerializeField, Required, ChildGameObjectsOnly] private SlotView _slot2;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _value;
        [SerializeField, Required, AssetsOnly] private Sprite _keySprite;
        [SerializeField, Required, AssetsOnly] private Sprite _resourceSprite;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _spriteRenderer;
        [SerializeField, Required, ChildGameObjectsOnly] private ActionTypeController _actionsCostController;
        [Inject] private readonly StatableManager _statableManager;

        public Stat Stat { get; private set; }
        public Transform StatTransform => transform;

        /*******************************************************************/
        public void Init(Card card)
        {
            _statableManager.Add(this);
            SetSlots(card);
            _actionsCostController.Init(card);
            _spriteRenderer.sprite = _resourceSprite;
            if (card is IPlayableFromHandInTurn playableFromHand) SetCostWith(playableFromHand.ResourceCost);
            else if (card is CardConditionReaction cardConditionFast) SetCostWith(cardConditionFast.ResourceCost);
            else if (card is CardGoal cardGoal)
            {
                _spriteRenderer.sprite = _keySprite;
                SetCostWith(cardGoal.Keys);
            }
            else gameObject.SetActive(false);
        }

        private void SetCostWith(Stat stat)
        {
            Stat = stat;
            gameObject.SetActive(true);
            UpdateAnimation();
        }

        public Tween UpdateAnimation()
        {
            _value.text = Stat.Value.ToString();
            return DOTween.Sequence();
        }

        private void SetSlots(Card card)
        {
            if (card.Info.Slots == null || card.Info.Slots.Length == 0)
            {
                _slot1.gameObject.SetActive(false);
                _slot2.gameObject.SetActive(false);
                return;
            }

            SlotType slot1 = card.Info.Slots.Length > 0 ? card.Info.Slots[0] : SlotType.None;
            SlotType slot2 = card.Info.Slots.Length > 1 ? card.Info.Slots[1] : SlotType.None;
            _slot1.Init(slot1);
            _slot2.Init(slot2);
        }
    }
}
