using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class AddSlotGameAction : GameAction
    {
        public Investigator Investigator { get; }
        public Slot Slot { get; }

        /*******************************************************************/
        public AddSlotGameAction(Investigator cardSupply, Slot slot)
        {
            Investigator = cardSupply;
            Slot = slot;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Investigator.SlotsCollection.AddSlot(Slot);
            await Task.CompletedTask;
        }

        public override async Task Undo()
        {
            Investigator.SlotsCollection.RemoveSlot(Slot);
            await Task.CompletedTask;
        }
    }
}
