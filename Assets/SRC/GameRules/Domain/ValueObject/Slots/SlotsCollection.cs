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

        public IEnumerable<SlotType> AllSlotsType => Slots.Where(slot => slot.IsEfective).Select(slot => slot.Type);

        public void AddSlot(Slot slot) => Slots.Add(slot);
    }
}
