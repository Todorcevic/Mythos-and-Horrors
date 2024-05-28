using System;

namespace MythosAndHorrors.GameRules
{
    public class Slot
    {
        public SlotType Type { get; }
        private Func<bool> Condition { get; set; }
        public bool IsEfective => Condition == null || Condition();

        /*******************************************************************/
        public Slot(SlotType slotType, Func<bool> condition = null)
        {
            Type = slotType;
            Condition = condition;
        }
    }
}
