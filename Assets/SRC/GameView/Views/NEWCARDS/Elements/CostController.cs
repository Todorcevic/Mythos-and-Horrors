using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

namespace MythosAndHorrors.GameView.NEWS
{
    public class CostController : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _cost;
        [SerializeField, Required, ChildGameObjectsOnly] private SlotView _slot1;
        [SerializeField, Required, ChildGameObjectsOnly] private SlotView _slot2;

        /*******************************************************************/
        public void Init(Card card)
        {
            if (card is IPlayableFromHand platableFromHand)
                SetCostWith(platableFromHand.ResourceCost);
            else if (card is CardGoal cardGoal)
                SetCostWith(cardGoal.Hints);

            SetSlots(card);
        }

        private void SetCostWith(Stat stat)
        {
            _cost.SetStat(stat);
            _cost.gameObject.SetActive(true);
        }

        private void SetSlots(Card card)
        {
            SlotType slot1 = card.Info.Slots.FirstOrDefault();
            SlotType slot2 = card.Info.Slots.LastOrDefault();
            _slot1.Init(slot1);
            _slot2.Init(slot2);
        }
    }
}
