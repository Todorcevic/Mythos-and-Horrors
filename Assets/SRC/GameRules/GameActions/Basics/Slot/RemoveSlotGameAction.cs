using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class RemoveSlotGameAction : GameAction
    {
        public Investigator Investigator { get; }
        public Slot Slot { get; }

        /*******************************************************************/
        public RemoveSlotGameAction(Investigator investigator, Slot slot)
        {
            Investigator = investigator;
            Slot = slot;
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
