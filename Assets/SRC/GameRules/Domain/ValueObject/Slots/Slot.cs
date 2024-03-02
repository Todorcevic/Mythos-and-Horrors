using System.Linq;

namespace MythsAndHorrors.GameRules
{
    public class Slot
    {
        public SlotType Type { get; }
        public Card FilledByThisCard { get; private set; }
        public bool IsEmpty => FilledByThisCard == null;

        /*******************************************************************/
        public Slot(SlotType slotType)
        {
            Type = slotType;
        }

        /*******************************************************************/
        public bool CanAddThis(Card card)
        {
            if (!IsEmpty) return false;
            if (!card.Info.Slots.Contains(Type)) return false;

            return SpecialCondition(card);
        }

        public void FillWith(Card card)
        {
            FilledByThisCard = card;
        }

        public void Clear()
        {
            FilledByThisCard = null;
        }

        protected virtual bool SpecialCondition(Card card) => true;

    }
}
