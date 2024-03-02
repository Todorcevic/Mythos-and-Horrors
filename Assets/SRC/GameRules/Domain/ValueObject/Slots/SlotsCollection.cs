using System.Collections.Generic;
using System.Linq;

namespace MythosAndHorrors.GameRules
{
    public class SlotsCollection
    {
        public List<Slot> Slots { get; private set; } = new()
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
                Slot freeSlot = Slots.Except(slots).FirstOrDefault(SlotType => SlotType.CanAddThis(card));
                if (freeSlot != null) slots.Add(freeSlot);
            }

            return slots;
        }

        public bool CanAddThis(Card card) => GetFreeSlotFor(card).Count >= card.Info.Slots.Count();

        public void AddSlot(Slot slot) => Slots.Add(slot);
    }
}
