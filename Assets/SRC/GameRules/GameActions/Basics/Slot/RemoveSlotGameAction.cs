using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class RemoveSlotGameAction : GameAction
    {
        public Investigator Investigator { get; private set; }
        public Slot Slot { get; private set; }

        /*******************************************************************/
        public RemoveSlotGameAction SetWith(Investigator investigator, Slot slot)
        {
            Investigator = investigator;
            Slot = slot;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Investigator.SlotsCollection.RemoveSlot(Slot);
            await Task.CompletedTask;
        }

        public override async Task Undo()
        {
            Investigator.SlotsCollection.AddSlot(Slot);
            await Task.CompletedTask;
        }
    }
}
