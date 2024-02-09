namespace MythsAndHorrors.GameRules
{
    public class Slot
    {
        public SlotType Type { get; private set; }
        public Card FilledByThisCard { get; private set; }
        public bool IsEmpty => FilledByThisCard == null;

        /*******************************************************************/
        public Slot(SlotType slotType)
        {
            Type = slotType;
        }

        /*******************************************************************/
        public bool CanAddThis(SlotType slotType)
        {
            if (!IsEmpty) return false;
            if (slotType != Type) return false;

            return SpecialCondition();
        }

        public void FillWith(Card card)
        {
            FilledByThisCard = card;
        }

        protected virtual bool SpecialCondition() => true;

    }
}
