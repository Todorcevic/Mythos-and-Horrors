using Sirenix.Utilities;
using System.Collections.Generic;

namespace MythsAndHorrors.GameRules
{
    public class SlotsManager
    {
        public List<Slot> Slots { get; private set; } = new List<Slot>()
        {
            new(SlotType.Trinket),
            new(SlotType.Equipment),
            new(SlotType.Supporter),
            new(SlotType.Item),
            new(SlotType.Item),
            new(SlotType.Magical),
            new(SlotType.Magical)
        };

        /*******************************************************************/
        public List<Slot> GetFreeSlotFor(Card card)
        {
            List<Slot> slots = new();

            foreach (SlotType slot in card.Info.Slots)
            {
                Slot freeSlot = Slots.Find(SlotType => SlotType.CanAddThis(slot));
                if (freeSlot != null) slots.Add(freeSlot);
            }

            return slots;
        }

        public void AddSlot(Slot slot) => Slots.Add(slot);
    }
}
