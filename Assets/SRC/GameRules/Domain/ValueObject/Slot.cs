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
        public void FillWith(Card card)
        {
            if (FilledByThisCard != null)
            {
                throw new System.Exception("Slot already filled");
            }
            FilledByThisCard = card;
        }
    }
}
