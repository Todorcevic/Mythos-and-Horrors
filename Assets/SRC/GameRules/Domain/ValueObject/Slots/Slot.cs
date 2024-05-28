using System;
using System.Linq;

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

        /*******************************************************************/
        //public bool CanAddThis(Card card)
        //{
        //    if (!IsEmpty) return false;
        //    if (!card.Info.Slots.Contains(Type)) return false;

        //    return SpecialCondition(card);
        //}

        //public void FillWith(Card card)
        //{
        //    FilledByThisCard = card;
        //}

        //public void Clear()
        //{
        //    FilledByThisCard = null;
        //}

        //protected virtual bool SpecialCondition(Card card) => true;

    }
}
